using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Commands and their settings are tightly coupled 1:1, so it makes perfect sense to nest the settings.")]
