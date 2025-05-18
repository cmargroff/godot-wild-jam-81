shader_type spatial;


varying vec3 world_pos;

uniform sampler2D noise;
uniform sampler2D noise_normal;

uniform float wave_time;
uniform float speed = 0.01;


void vertex() {
	
	
	world_pos = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;
	
	VERTEX.y = texture(noise, world_pos.xz / 500.0 + wave_time * speed).r * 20.0;
	
}

void fragment() {
	
	ALBEDO = vec3(0, 0.2, 0.3);
	ROUGHNESS = 0.05;
	
	NORMAL_MAP = texture(noise_normal, world_pos.xz / 500.0 + wave_time * speed).rgb;
	
}
