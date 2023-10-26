[![npm package](https://img.shields.io/npm/v/com.m3taphysics.Arch.SystemGroups.Visualiser)](https://www.npmjs.com/package/com.m3taphysics.Arch.SystemGroups.Visualiser)
[![openupm](https://img.shields.io/npm/v/com.m3taphysics.Arch.SystemGroups.Visualiser?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.m3taphysics.Arch.SystemGroups.Visualiser/)
![Tests](https://github.com/m3taphysics/Arch.SystemGroups.Visualiser/workflows/Tests/badge.svg)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)

# Visualiser

Visualises Arch.SystemGroups system dependencies

- [How to use](#how-to-use)
- [Install](#install)
  - [via npm](#via-npm)
  - [via OpenUPM](#via-openupm)
  - [via Git URL](#via-git-url)
  - [Tests](#tests)
- [Configuration](#configuration)

<!-- toc -->

## How to use

*Work In Progress*

## Install

### via npm

Open `Packages/manifest.json` with your favorite text editor. Add a [scoped registry](https://docs.unity3d.com/Manual/upm-scoped.html) and following line to dependencies block:
```json
{
  "scopedRegistries": [
    {
      "name": "npmjs",
      "url": "https://registry.npmjs.org/",
      "scopes": [
        "com.m3taphysics"
      ]
    }
  ],
  "dependencies": {
    "com.m3taphysics.Arch.SystemGroups.Visualiser": "1.0.0"
  }
}
```
Package should now appear in package manager.

### via OpenUPM

The package is also available on the [openupm registry](https://openupm.com/packages/com.m3taphysics.Arch.SystemGroups.Visualiser). You can install it eg. via [openupm-cli](https://github.com/openupm/openupm-cli).

```
openupm add com.m3taphysics.Arch.SystemGroups.Visualiser
```

### via Git URL

Open `Packages/manifest.json` with your favorite text editor. Add following line to the dependencies block:
```json
{
  "dependencies": {
    "com.m3taphysics.Arch.SystemGroups.Visualiser": "https://github.com/m3taphysics/Arch.SystemGroups.Visualiser.git"
  }
}
```

### Tests

The package can optionally be set as *testable*.
In practice this means that tests in the package will be visible in the [Unity Test Runner](https://docs.unity3d.com/2017.4/Documentation/Manual/testing-editortestsrunner.html).

Open `Packages/manifest.json` with your favorite text editor. Add following line **after** the dependencies block:
```json
{
  "dependencies": {
  },
  "testables": [ "com.m3taphysics.Arch.SystemGroups.Visualiser" ]
}
```

## Configuration

*Work In Progress*

## License

MIT License

Copyright © 2023 m3taphysics
