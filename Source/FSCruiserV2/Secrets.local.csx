Output.WriteLine($@"
namespace FSCruiser.WinForms
{{
    public static partial class Secrets
    {{
		static Secrets()
		{{
			APPCENTER_KEY_WINDOWS = ""{System.Environment.GetEnvironmentVariable("fscruiserv2_appcenterr_key_windows") ?? ""}"";
		}}
    }}
}}
");