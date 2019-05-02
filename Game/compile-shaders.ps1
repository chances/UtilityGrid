
$shaders = Get-ChildItem -Path "$PSScriptRoot\Content\Shaders\*.hlsl" -Recurse -Force -Name

foreach ($shader in $shaders) {
    $filenameWithoutExt = [IO.Path]::GetFileNameWithoutExtension($shader)
    Invoke-Expression "glslangValidator -S vert -e VS -V -D '$PSScriptRoot\Content\Shaders\$shader' -o '$PSScriptRoot\Content\Shaders\$filenameWithoutExt.vs.spirv'"
    Invoke-Expression "glslangValidator -S frag -e FS -V -D '$PSScriptRoot\Content\Shaders\$shader' -o '$PSScriptRoot\Content\Shaders\$filenameWithoutExt.vs.spirv'"
}
