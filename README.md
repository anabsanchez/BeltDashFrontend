# BeltDashFrontend

Frontend for _Belt Dash_, a 2D endless flyer game developed with Godot 4.3 and C#.  
This project includes the core gameplay, visual interface, and a built-in admin panel for managing user accounts directly from within the game.

## 📦 Features

- Endless flyer gameplay with smooth parallax scrolling
- User login and registration interface
- Global ranking system and personal best tracking
- Role-based UI: Player and Admin views
- Integrated admin panel (block, delete, promote users)
- Designed with a custom space-themed UI and color palette
- Frontend built entirely with Godot 4.3 using C#

## 🖥️ Platforms

Compatible with **Windows** and **Linux**.  
Export templates and project settings are optimized for desktop environments.

## 📁 Project Structure

- `scenes/` — All UI and gameplay scenes
- `scripts/` — C# scripts for player logic, authentication, API integration, etc.
- `assets/` — Sprites, fonts, sound and visual effects
- `config/` — Game settings, export presets, localization (if applicable)

## 🔗 Backend

This frontend connects to a RESTful API developed in [ASP.NET Core](https://dotnet.microsoft.com/), using Entity Framework Core for database operations.

> The backend is hosted in a separate repository:  
> [BeltDashBackend (link here)]

## 📄 License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.
