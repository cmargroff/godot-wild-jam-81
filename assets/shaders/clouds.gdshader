shader_type spatial;
render_mode cull_disabled, depth_prepass_alpha, unshaded;

varying vec3 world_pos;

uniform sampler2D noise1;
uniform float noise1_scale;


void vertex() {
	
	world_pos = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;
	
	
}

void fragment() {
	
	
	ALBEDO = texture(noise1, world_pos.xz / noise1_scale).rgb;
	ALPHA = texture(noise1, world_pos.xz / noise1_scale).r;
	
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
