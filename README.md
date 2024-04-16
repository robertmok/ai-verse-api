# AI Verse API

The API for [AI Verse](https://github.com/robertmok/ai-verse). It uses **ASP.NET Core Web API** and **SignalR** for real time data.

![](./demo.JPG)

## Setup

### Prerequisites

- Install [Ollama](https://ollama.com/)
- Download [gemma:2b](https://ollama.com/library/gemma)
- Download [orca-mini:3b](https://ollama.com/library/orca-mini)

### Quickstart

- Have **Ollama** running
	- Ollama REST API runs on http://localhost:11434
- Run the project on **https**
	- Swagger will be running on https://localhost:7202/swagger/index.html
	- SignalR hub will be running on https://localhost:7202/hub
- Follow the **Setup** section in [AI Verse](https://github.com/robertmok/ai-verse)

## Limitations

- No cancellation when it is generating response.
- The response are send globally to all connected clients in SignalR. It is not client or connection specific.

## Future

- Alternative to not use SignalR and just send server-sent events (event-stream) from API.
- Add support to use any LLMs
- Dockerfile

