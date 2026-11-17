using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

internal class RunnerInput
{
    public string ServerDir { get; set; } = "";
    public string InstallDir { get; set; } = "";
    public string DataDir { get; set; } = "";
    public string Codename { get; set; } = "";
    public string Method { get; set; } = "";
    public bool Optional { get; set; }
    public string DllPath { get; set; } = "";
    public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
}

internal static class Program
{
    private static void PopulateContext(object ctxObj, RunnerInput input)
    {
        var t = ctxObj.GetType();
        t.GetProperty("ServerDir")?.SetValue(ctxObj, input.ServerDir ?? "");
        t.GetProperty("InstallDir")?.SetValue(ctxObj, input.InstallDir ?? "");
        t.GetProperty("DataDir")?.SetValue(ctxObj, input.DataDir ?? "");
        t.GetProperty("Codename")?.SetValue(ctxObj, input.Codename ?? "");

        var propsProp = t.GetProperty("Properties");
        if (propsProp?.GetValue(ctxObj) is IDictionary<string, string> dict)
        {
            dict.Clear();
            if (input.Properties != null)
            {
                foreach (var kvp in input.Properties)
                {
                    dict[kvp.Key] = kvp.Value ?? "";
                }
            }
        }
        else if (propsProp != null && input.Properties != null)
        {
            propsProp.SetValue(ctxObj, new Dictionary<string, string>(input.Properties, StringComparer.OrdinalIgnoreCase));
        }
    }

    private static Dictionary<string, string> ReadContextProperties(object? ctxObj, Dictionary<string, string> fallback)
    {
        if (ctxObj == null) return fallback;
        var propsProp = ctxObj.GetType().GetProperty("Properties");
        if (propsProp?.GetValue(ctxObj) is IDictionary<string, string> dict)
        {
            return new Dictionary<string, string>(dict, StringComparer.OrdinalIgnoreCase);
        }
        return fallback;
    }

    public static async Task<int> Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.Error.WriteLine("Runner requires input and output JSON paths.");
            return 2;
        }

        string inputJson = File.ReadAllText(args[0]);
        var input = JsonSerializer.Deserialize<RunnerInput>(inputJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new RunnerInput();

        var asm = !string.IsNullOrWhiteSpace(input.DllPath)
            ? Assembly.LoadFrom(input.DllPath)
            : Assembly.GetExecutingAssembly();

        string expectedClass = (input.Codename ?? "") + "_funcs";
        Type? funcsType = asm.GetTypes().FirstOrDefault(t =>
            t.IsClass && t.IsPublic && t.Namespace == "ServerCreationTool.Servers" &&
            string.Equals(t.Name, expectedClass, StringComparison.OrdinalIgnoreCase))
            ?? asm.GetTypes().FirstOrDefault(t =>
                t.IsClass && t.IsPublic && t.Namespace == "ServerCreationTool.Servers" &&
                t.Name.EndsWith("_funcs", StringComparison.OrdinalIgnoreCase));

        if (funcsType == null)
        {
            Console.Error.WriteLine($"No *_funcs class found for {expectedClass}.");
            return 3;
        }

        object? instance = Activator.CreateInstance(funcsType);
        if (instance == null)
        {
            Console.Error.WriteLine($"Failed to instantiate {funcsType.Name}.");
            return 4;
        }

        PropertyInfo? ctxProp = funcsType.GetProperty("Context", BindingFlags.Public | BindingFlags.Instance);
        object? ctxObj = ctxProp?.GetValue(instance);
        if (ctxObj == null && ctxProp != null)
        {
            ctxObj = Activator.CreateInstance(ctxProp.PropertyType);
            if (ctxObj != null)
            {
                ctxProp.SetValue(instance, ctxObj);
            }
        }
        if (ctxObj != null)
        {
            PopulateContext(ctxObj, input);
        }

        MethodInfo? method = funcsType.GetMethod(input.Method ?? "", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        if (method == null)
        {
            if (input.Optional)
            {
                var outProps = ReadContextProperties(ctxObj, input.Properties ?? new Dictionary<string, string>());
                File.WriteAllText(args[1], JsonSerializer.Serialize(outProps));
                return 0;
            }
            Console.Error.WriteLine($"Function '{input.Method}' was not found in {funcsType.Name}.");
            return 5;
        }

        object? ret = method.Invoke(instance, Array.Empty<object>());
        if (ret is Task task)
        {
            await task;
        }

        var finalCtxObj = ctxProp?.GetValue(instance) ?? ctxObj;
        var finalProps = ReadContextProperties(finalCtxObj, input.Properties ?? new Dictionary<string, string>());
        File.WriteAllText(args[1], JsonSerializer.Serialize(finalProps));
        return 0;
    }
}
