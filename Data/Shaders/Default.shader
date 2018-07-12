shader_type spatial;

uniform vec4 color : hint_color;

void fragment() {
	ALBEDO = color.rgb;
	METALLIC = 0.0;
	SPECULAR = 0.0;
	ROUGHNESS = 1.0;
//	EMISSION = color.rgb * 0.5;
}

void light() {
//    DIFFUSE_LIGHT = LIGHT_COLOR * ALBEDO * 0.2;
}
