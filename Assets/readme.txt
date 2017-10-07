Check Configs folder for sample generators settings
Settings files go into slots on ConfigHolder object (top of the hierarchy)

road_lines	- simple line packing of randomly resized objects, demo, not pretty
road_mix	- adds random positioning for nicer picture, kinda strongly depends on camera angle

Deco generator can be extended to spawn fields with arbitrary number of rows by adding more DecoRow objects to the processed list