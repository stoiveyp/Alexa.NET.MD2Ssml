# Alexa.NET.MD2Ssml
Converts markdown syntax to the Alexa.NET SSML object model, allowing expressive Alexa speech to be stored within modern CMS systems.

(Please, if you find this project useful, contact me to let me know or raise PRs/issues to help me improve!)

## Current Support
| Markdown  | SSML |
| ------------- | ------------- |
| Paragraph  | Paragraph  |
| Soft Break  | Sentence  |
| Hard Break  | Sentence  |
| Emphasis  |  Prosody  -  pitch set to Extra High |
| Strong  |  Emphasis  -  level set to strong |

## Example

To translate your markdown, just use the MarkdownConverter class

```csharp
var speech = MarkdownConverter.Convert("**hello** world");
return ResponseBuilder.Tell(speech);
```
