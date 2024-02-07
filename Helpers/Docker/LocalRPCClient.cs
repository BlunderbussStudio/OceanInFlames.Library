using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace OceansInFlame.Library.Helpers.Docker
{
    public readonly record struct ImageInfo(string id);
    public readonly record struct ContainerInfo(string id, string image, string state, string status);

    public class LocalRPCClient
    {
        DockerClient CreateClient() => new DockerClientConfiguration(new Uri("http://127.0.0.1:2375")).CreateClient();

        public async Task Ping()
        {
            var client = CreateClient();
            var system = client.System;
            await system.PingAsync();
        }

        public async IAsyncEnumerable<ImageInfo> ListImages()
        {
            var client = CreateClient();
            var query = new ImagesListParameters();
            var images = await client.Images.ListImagesAsync(query);
            foreach (var image in images)
            {
                yield return new ImageInfo(image.ID);
            }
        }

        public async IAsyncEnumerable<ContainerInfo> ListContainers(string imageId)
        {
            var client = CreateClient();
            var query = new ContainersListParameters();
            var containers = await client.Containers.ListContainersAsync(query);
            foreach (var container in containers)
            {
                yield return new ContainerInfo(container.ID, container.ImageID, container.State, container.Status);
            }
        }

        public async Task StartContainer(string imageId, string mountVolume, IDictionary<string, string> labels, IDictionary<int, int> ports)
        {
            var client = CreateClient();
            var query = new CreateContainerParameters();
            query.Image = imageId;
            query.Labels = labels;
            query.Env.Add("LABEL=" + mountVolume);
            client.Containers.CreateContainerAsync(query);
        }
    }
}
