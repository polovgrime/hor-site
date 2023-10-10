using Newtonsoft.Json;
using System.Text;

namespace PortfolioWebServer.Services
{
    internal class State
    {
        public IEnumerable<Link> Links { get; }

        public IEnumerable<ProjectDto> Projects { get; } 

        public State(IEnumerable<Link> links, IEnumerable<ProjectDto> projects)
        {
            Links = links;
            Projects = projects;
        }
    }

    internal static class StateService
    {
        private const string StateTemplate = @"
            const state = JSON.parse('{0}');
        ";

        public static async Task CreateState()
        {
            if (File.Exists("./src/state.js") == false)
            {
                File.Create("./src/state.js").Close();
            }
            var links = new LinksService().RetrieveLinks();
            var projects = await new ProjectsService().RetrieveProjects();

            var state = new State(links, projects);

            var template = string.Format(StateTemplate, JsonConvert.SerializeObject(state).Replace("'", "\\'"));

            File.WriteAllText("./src/state.js", template); 
        }
    }
}
