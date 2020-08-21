# VBRCheck
A Mapset Verifier plugin to check VBR quality settings.

<center><img src="https://cdn.discordapp.com/attachments/745940041963929651/746284182786932816/unknown.png"></img></center>

## Installing
**Disclaimer**: This is extremely hacky.

1. Download [Audacity's FFmpeg library](https://manual.audacityteam.org/man/faq_installing_the_ffmpeg_import_export_library.html). (You can use latest FFmpeg, however it's 2x bigger.)
2. Extract it to `%APPDATA%\Mapset Verifier Externals\ffmpeg`. `%APPDATA%\Mapset Verifier Externals\ffmpeg\ffmpeg.exe` should exist now.
3. Go to release tab, download `VBRCheck.dll` and `FFmpeg.NET.dll`.
4. Put `VBRCheck.dll` to `%APPDATA%\Mapset Verifier Externals\checks`.
5. Put `FFmpeg.NET.dll` to `Program Files\Mapset Verifier\resources\app\api\win-x86`.
6. Open `Program Files\Mapset Verifier\resources\app\api\win-x86\MapsetVerifierBackend.deps.json` in your text editor.
7. Apply these `diff`s being done to the file.
```diff
          "runtimepack.Microsoft.NETCore.App.Runtime.win-x86": "3.1.2",
+		  "xFFmpeg.NET": "3.4.0"
```

```diff
      "ManagedBass/2.0.4": {
        "runtime": {
          "lib/netstandard1.4/ManagedBass.dll": {
            "assemblyVersion": "1.0.0.0",
            "fileVersion": "1.0.0.0"
          }
        }
      },
+	  "xFFmpeg.NET/3.4.0": {
+        "runtime": {
+          "lib/netstandard2.0/FFmpeg.NET.dll": {
+            "assemblyVersion": "3.4.0.0",
+            "fileVersion": "3.4.0.0"
+          }
+        }
+      },
```

```diff
    "runtimepack.Microsoft.NETCore.App.Runtime.win-x86/3.1.2": {
      "type": "runtimepack",
      "serviceable": false,
      "sha512": ""
    },
+	"xFFmpeg.NET/3.4.0": {
+      "type": "package",
+      "serviceable": true,
+      "sha512": "sha512-gUWRBhI7XDsnCEP9dFf/DRM6AINQsZngr7K99LD/u2e2TAOCg152ntquLbhplyXjRczu9Ytm87a5EdS9G5Dqzw==",
+      "path": "xffmpeg.net/3.4.0",
+      "hashPath": "xffmpeg.net.3.4.0.nupkg.sha512"
+    },
```
8. Done.