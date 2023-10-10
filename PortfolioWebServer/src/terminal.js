const { links, projects } = state;

const commands = {
    'about': composeAbout,
    'links': drawLinks,
    'openlink': openLink,
    'projects': composeProjects
};

commands['help'] = () => draw(Object.keys(commands).join(', '));

const input = document.getElementById('input');
const terminal = document.getElementById('terminal');
const usedCommands = [];
let currentCmdIndex = 0;

input.focus();
input.addEventListener('keyup', function (event) {
    if (event.key === 'Enter') {
        resolveCommand(input.value);
    }

    if (event.key === 'ArrowUp') {
        prevCommand();
    }

    if (event.key === 'ArrowDown') {
        nextCommand();
    }
});

function resolveCommand(command) {
    clearInput();
    addCommand(command);

    var attrs = command.split(" ");

    if (attrs[0] == "clear") {
        clear();
        return;
    }

    if (commands.hasOwnProperty(attrs[0]) == false) {
        commandNotFound(attrs[0]);
    } else {
        commands[attrs[0]](attrs);
    }
}

function commandNotFound(name) {
    draw('Command ' + name + ' was not found');
}

function draw(text) {
    let newElement = document.createElement('div');
    newElement.innerHTML = text;
    drawElement(newElement);
}

function drawElement(el) {
    terminal.appendChild(el);
    terminal.scrollTop = terminal.scrollHeight;
    terminal.appendChild(document.createElement('br'));
}

function clear() {
    terminal.innerHTML = "";
}

function clearInput() {
    input.value = "";
}

function addCommand(command) {
    if (command == usedCommands[usedCommands.length - 1]) {
        return;
    }
    usedCommands.push(command);
    currentCmdIndex = usedCommands.length;
}

function prevCommand() {
    currentCmdIndex--;
    if (currentCmdIndex <= 0) {
        currentCmdIndex = 0;
    }

    typeCommand();
}

function nextCommand() {
    currentCmdIndex++;
    if (currentCmdIndex >= usedCommands.length) {
        currentCmdIndex = usedCommands.length - 1;
    }

    typeCommand();
}

function typeCommand() {
    if (usedCommands.length == 0) {
        return;
    }

    input.value = usedCommands[currentCmdIndex];
}

function composeAbout() {
    const aboutMe = 'My name\'s Anton, I\'m a 26 years old programmer. Currently a .NET backend developer, who wishes to become a professional gamedev.';
    const myStack = 'Have experience in .NET (through 3.5 to 7). JavaScript, vue.js, node.js (not proud). Unity, but not deep. Godot (same thing, but I usually use any game engine as a platform to recreate game mechanics, not really into any real engine coding yet)';
    const aboutThisWebsite = 'This website is made with no frameworks on frontend or backend. Plain html js css on frontend and .NET on backend,' +
        'not really that pure since it\'s .NET, but still. Custom webserver. Currently I\'m working on my own http1.1 (then http2) implementation';

    draw([aboutMe, myStack, aboutThisWebsite].join('</br>'));
}

function drawLinks() {
    draw(links.map(i => `<a href="${i.url}">${i.title}</a>`).join('</br>'));
}

function openLink(attrs) {
    let link = links.filter(link => link.title == attrs[1])[0];

    if (link === undefined) {
        draw(`Link ${attrs[1]} was not found`);
        return;
    }

    draw(`Opening ${link.title}...`);
    window.open(link.url, '_blank');
}

function composeProjects(attrs) {
    let targetProject;

    if (attrs[1] !== undefined) {
        displayProjectByTitle(attrs[1]);
        return;
    }

    let composed = projects.map(project => {
        let projectDiv = document.createElement('div');
        let title = document.createElement('a');
        title.href = project.url;
        title.target = '_blank';
        title.innerHTML = project.title;

        projectDiv.appendChild(title);

        let description = document.createElement('span');
        let descriptionText = ': ' + project.description;

        if (descriptionText.length > 50) {
            descriptionText = descriptionText.substring(0, 50) + '... '
        }

        description.innerHTML = descriptionText;

        projectDiv.appendChild(description);

        let updated = document.createElement('div');

        updated.innerHTML = 'Last updated: ' + project.updated;

        projectDiv.appendChild(updated);

        let expandButton = document.createElement('a');

        expandButton.onclick = () => displaySingleProject(project);
        expandButton.href = '#';
        expandButton.innerHTML = 'Expand';
        expandButton.className = 'expand-link';

        projectDiv.appendChild(expandButton);

        return projectDiv;
    });

    composed.forEach(project => drawElement(project));
}

function displayProjectByTitle(projectTitle) {
    const targetProject = projects.filter(project => project.title == projectTitle);

    if (targetProject[0] === undefined) {
        draw(`Project ${projectTitle} was not found`);
        return;
    }

    displaySingleProject(targetProject[0]);
}

function displaySingleProject(project) {
    let projectDiv = document.createElement('div');
    let title = document.createElement('a');
    title.href = project.url;
    title.innerHTML = project.title;

    projectDiv.appendChild(title);
    projectDiv.appendChild(document.createElement('br'));

    let description = document.createElement('div');
    description.innerHTML = project.description;

    projectDiv.appendChild(description);

    let updated = document.createElement('div');
    updated.innerHTML = 'Last updated: ' + project.updated;

    projectDiv.appendChild(updated);

    drawElement(projectDiv);
}