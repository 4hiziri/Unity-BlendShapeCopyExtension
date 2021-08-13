# BlendShapeCopyTool
This Unity extension assists in copying blendshape values to other Skinned Mesh Render's blendshape.

# How To Use
1. Right click on Skinned Mesh Render you want to copy from
2. Click "Copy BlendShapes"
3. Select the mesh you want to copy to
4. Right click on its mesh's Skinned Mesh Render
5. Click "Paste BlendShapes Strictly", "Paste BlendShapes By Name" or "Paste BlendShapes Only Non-Zero Values"

"Paste BlendShapes Strictly" checks that both meshes have same blendshape name and do nothing if not same.

"Paste BlendShapes By Name" copies blendshape values only when each name's is the same.

"Paste BlendShapes Only Non-Zero Values" copies blendshape values when it is not zero.