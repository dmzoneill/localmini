# Localmini

A Windows system tray utility for quickly sharing files, screenshots and
links.

## What it does

- Uploads files to the localhostr file host and keeps an upload history.
- Shortens URLs with the minihref service and keeps a link history.
- Takes screenshots, including bitmaps of open web pages.
- Picks up URLs from the clipboard and from open Internet Explorer
  windows.
- Emails links to saved contacts over SMTP.
- Adds an **Upload With Localmini** entry to the Explorer context menu
  and supports global hotkeys for uploading, screenshots, shortening and
  email.
- Stores its preferences in the registry.

The repository holds two versions of the project, `localmini1` and
`localmini2`.

## Tech

- C# (Windows Forms)
- .NET Framework

## Building

Open the `.sln` file in either version folder in Visual Studio and
build it. Windows only.

## License

Licensed under the Apache License 2.0. See [LICENSE](LICENSE).
