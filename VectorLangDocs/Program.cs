using System.Reflection;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using VectorLangDocs;
using VectorLangDocs.Shared.DocumentationModel;

var docs = VectorLangDocumentation.FromXml(ReadEmbeddedResource("VectorLang/docs/en.xml"));

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

await builder.Build().RunAsync();

string ReadEmbeddedResource(string resourceName)
{
    var assembly = Assembly.GetExecutingAssembly()!;

    using Stream stream = assembly.GetManifestResourceStream(resourceName)!;
    using StreamReader reader = new StreamReader(stream);

    return reader.ReadToEnd();
}
