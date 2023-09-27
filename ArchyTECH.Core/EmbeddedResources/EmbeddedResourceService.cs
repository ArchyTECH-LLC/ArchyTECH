using System.Reflection;

namespace ArchyTECH.Core.EmbeddedResources
{
    public interface IEmbeddedResourceService
    {
        Stream GetEmbeddedFileStreamByAssemblyType<TAssemblyType>(string resourceFilePath);
        Stream GetEmbeddedFileStream(Assembly assembly, string resourceFilePath);
        string GetEmbeddedFileContentByAssemblyType<TAssemblyType>(string resourceFilePath);
        string GetEmbeddedFileContent(Assembly assembly, string resourceFilePath);
        Byte[] GetEmbeddedFileBytesByAssemblyType<TAssemblyType>(string resourceFilePath);
        Byte[] GetEmbeddedFileBytes(Assembly assembly, string resourceFilePath);
    }

    public class EmbeddedResourceService : IEmbeddedResourceService
    {

        public Stream GetEmbeddedFileStreamByAssemblyType<TAssemblyType>(string resourceFilePath)
        {
            return GetEmbeddedFileStream(typeof(TAssemblyType).Assembly, resourceFilePath);
        }
        public Stream GetEmbeddedFileStream(Assembly assembly, string resourceFilePath)
        {

            var path = $"{assembly.GetName().Name}.{resourceFilePath}";
            var stream = assembly
                    .GetManifestResourceStream(path);

            return stream ?? throw new InvalidOperationException($"Unable to find embedded resource path: {path}");
        }

        public string GetEmbeddedFileContentByAssemblyType<TAssemblyType>(string resourceFilePath)
        {
            return GetEmbeddedFileContent(typeof(TAssemblyType).Assembly, resourceFilePath);
        }

        public string GetEmbeddedFileContent(Assembly assembly, string resourceFilePath)
        {
            using var reader = new StreamReader(GetEmbeddedFileStream(assembly, resourceFilePath));
            return reader.ReadToEnd();
        }


        public Byte[] GetEmbeddedFileBytesByAssemblyType<TAssemblyType>(string resourceFilePath)
        {
            return GetEmbeddedFileBytes(typeof(TAssemblyType).Assembly, resourceFilePath);
        }

        public Byte[] GetEmbeddedFileBytes(Assembly assembly, string resourceFilePath)
        {
            using var memoryStream = new MemoryStream();
            GetEmbeddedFileStream(assembly, resourceFilePath).CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
