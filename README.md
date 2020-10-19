# OgmoLoader

Load Ogmo file simply by using static method

```csharp
var level = OgmoLoader.LoadLevel("ogmo_level.json");
```

Load custom values from level or entity by using
OgmoValueContainer::GetIntValue(string) or OgmoValueContainer::GetStringValue(string)

```csharp
var level = OgmoLoader::LoadLevel("ogmo_level.json");
var padding = level.GetIntValue("padding");
```
