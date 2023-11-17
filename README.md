# Documentation Viewer - Удобная утилита для работы с документацией внутри игры!

# Лицензия
Выпускается под лицензией MIT, [подробности тут](./LICENSE).

# Интеграция с движками

## Unity
> Проверено на Unity 2020.3 (не зависит от нее) и содержит asmdef-описания для компиляции в виде отдельных сборок и уменьшения времени рекомпиляции основного проекта.

# Установка

## В виде unity модуля
Поддерживается установка в виде unity-модуля через git-ссылку в PackageManager:
```
"com.documentation.viewer.unity": "https://github.com/datuloar/documentation.viewer.git",
```

## Пример использования аттрибута
Первым параметром идет DocumentationCatalogType - это каталог в который будет помещена документация.
Второй параметр это текст документации.

```c#
[Documentation(DocumentationCatalogType.Ability, "Text documentation")]
class ExampleAbility {
    // some logic
}
```
## Окно с документацией
<p align="center">
    <img src="GitResources~/ToolsDocumentationViewer.jpg" width="1000" height="600" alt="Documentation window tutorial">
</p>

# TODO:
* Кеширование в json
* Экспорт документации в doc