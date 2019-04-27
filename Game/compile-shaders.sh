#!/usr/bin/env bash

scriptPath="`dirname \"$0\"`"

shaders=`find Content/Shaders -type f -name '*.hlsl' -exec basename {} \;`

for shader in $shaders; do
    filenameWithoutExt=${shader%.*}
    glslangValidator -S vert -e VS -V -D "Content/Shaders/"$shader -o "Content/Shaders/"$filenameWithoutExt".vs.spirv"
    glslangValidator -S frag -e FS -V -D "Content/Shaders/"$shader -o "Content/Shaders/"$filenameWithoutExt".fs.spirv"
done
