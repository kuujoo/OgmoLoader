# OgmoLoader

Load Ogmo file by simply using static function

```csharp
var level = OgmoLoader::LoadLevel("ogmo_level.json")
```

Load custom values from level or entity by using
OgmoValueContainer::GetIntValue(string) or OgmoValueContainer::GetStringValue(string)

```csharp
var level = OgmoLoader::LoadLevel("ogmo_level.json")
var padding = level.GetIntValue("padding");
```
