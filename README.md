# Lette.Wpf.AppCommands

MVVM-friendly attached behavior for `Window`, where you can bind WPF `ICommand`s to system-defined application commands.

> BIG NOTE: These are *not* the same as the WPF `ApplicationCommands`.

## Motivation

I wanted my WPF app to react to the "back" button on my mouse. Creating a custom `MouseBinding` and reacting to `XButton1` was fairly easy.

However, most mice with extra buttons can be customized through its accompanying software. Hence, the `XButton1` isn't _always_ the "back" button.

This package catches the _underlying_ system-defined application commands, and is therefore well-suited for handling `BrowserBackwards` and `BrowserForward` messages that may be triggered from any button on the mouse. (Or on any other type of specialized hardware, for that matter!)

## Usage

Add this XAML to your `MainWindow` (of type `Window`):

    <l:Window.AppCommandBindings>
        <AppCommandBinding AppCommand="BrowserForward" Command="{Binding ForwardCommand}" />
        <AppCommandBinding AppCommand="BrowserBackward" Command="{Binding BackwardCommand}" />
    </l:Window.AppCommandBindings>

This will invoke the `ICommand` in the `Command` property (with an optional `CommandParameter` value) whenever your `MainWindow` receives a corresponding application command.

## How it works (or _should_ work)

These bindings listen for `WM_APPCOMMAND` messages with an `lparam` containing an `APPCOMMAND_*` corresponding to the `AppCommand` value, as they come in on the `MainWindow` message loop (`WndProc`).

See [the docs on WM_APPCOMMAND](https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-appcommand) for more info on the available commands.

Note that these messages are usually broadcast by specialized hardware, ie. configurable extra buttons on your mouse or media keys on your keyboard.

Many mice with two extra buttons have those two buttons wired up for back and forward navigation out of the box.

## Known shortcomings

### Works only when your application is _active_.

The current version of this software will only detect these messages when your application is _active_.

Pressing the "back" button on your mouse while not hovering over your application will most likely activate whatever window is below the mouse cursor, hence _deactivating_ yours.

### Some messages never reach your app

Even if your app is active, some messages gets handled earlier by other software and are stopped from propagating.

Exactly why (shell hooks?) this happens, and how to deal with it, is unclear to me at the moment.

## Disclaimer

Technically, _all_ defined `APPCOMMAND_*` values are supported, but YMMV.
