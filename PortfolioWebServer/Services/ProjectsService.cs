using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace PortfolioWebServer.Services
{
    internal class Project
    {
        [JsonProperty("name")]
        public string? Title { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("html_url")]
        public string? Url { get; set; }

        [JsonProperty("updated_at")]
        public string? Updated { get; set; }
    }

    internal class ProjectDto
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Url { get; set; }

        public string? Updated { get; set; }

        public ProjectDto(Project project)
        {
            Title = project.Title;
            Description = project.Description;
            Url = project.Url;
            Updated = project.Updated;
        }
    }

    internal class ProjectsService
    {
        private const string apiUrl = "https://api.github.com/users/polovgrime/repos";

        public async Task<IEnumerable<ProjectDto>> RetrieveProjects()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders
                    .Add("User-Agent", 
                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36");

                var responseMessage = await client.GetAsync(apiUrl);
                var response = await responseMessage.Content.ReadAsStringAsync();
                var projects = JsonConvert.DeserializeObject<List<Project>>(response);

                if (projects == null)
                {
                    throw new Exception("Couldn't retrieve projects from github");
                }

                return projects
                    .Select(e => new ProjectDto(e))
                    .OrderBy(e => e.Updated);
            }
        }
    }
}
