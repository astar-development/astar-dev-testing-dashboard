namespace AStar.Dev.Testing.Dashboard.Services;

public class ThemeService
{
    public event Action? OnThemeChange;
    private string       currentTheme = "dark";

    public string CurrentTheme
    {
        get => currentTheme;
        set
        {
            if (currentTheme == value)
            {
                return;
            }

            currentTheme = value;
            OnThemeChange?.Invoke();
        }
    }
}
