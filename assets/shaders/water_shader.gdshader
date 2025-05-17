shader_type spatial;


uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_linear;
varying vec3 world_pos;

uniform vec3 water_color : source_color;

uniform float wave_time;
uniform float boat_speed;
varying vec2 boat_pos;

group_uniforms noisevalues;

uniform sampler2D noise1;
uniform sampler2D noise1_normal;
uniform sampler2D noise2;
uniform sampler2D noise2_normal;
uniform sampler2D noise3;
uniform sampler2D noise3_normal;
uniform float noise1_scale = 1.0;
uniform float noise2_scale = 1.0;
uniform float noise3_scale = 1.0;
uniform vec2 noise1_speed;
uniform vec2 noise2_speed;
uniform vec2 noise3_speed;
uniform float noise1_strength = 1.0;
uniform float noise2_strength = 1.0;
uniform float noise3_strength = 1.0;

group_uniforms depth;

uniform vec3 fresnel_color : source_color;

uniform sampler2D DEPTH_TEXTURE : hint_depth_texture;
uniform vec4 color_deep : source_color;
uniform vec4 color_shallow : source_color;
uniform float beers_law = 2.0;
uniform float depth_offset = -0.75;

group_uniforms edge;

uniform float edge_scale = 0.1;
uniform float near = 1.0;
uniform float far = 100.0;
uniform vec3 edge_color : source_color;

group_uniforms fade;

uniform float camera_pos;
uniform float distance_fade = 500;

varying float height;

float fresnel(float amount, vec3 normal, vec3 view){
	return pow((1.0 - clamp(dot(normalize(normal), normalize(view)), 0.0, 1.0)), amount);
}

float edge(float depth){
	depth = 1.0 - 2.0 * depth;
	return near * far / (far + depth * (near - far));
}



void vertex() {
	
	world_pos = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;
	
	boat_pos = vec2(boat_speed * wave_time, 0.0);
	
	height = texture(noise1, world_pos.xz / noise1_scale + wave_time * noise1_speed + boat_pos).r * noise1_strength;
	height += texture(noise2, world_pos.xz / noise2_scale + wave_time * noise2_speed + boat_pos).r * noise2_strength;
	height += texture(noise3, world_pos.xz / noise3_scale + wave_time * noise3_speed + boat_pos).r * noise3_strength;
	VERTEX.y = height;
	
}


void fragment() {
	vec3 asd;
	ALBEDO = water_color;
	ROUGHNESS = 0.1;
	SPECULAR = 0.9;
	METALLIC = 0.0;
	RIM = 0.3;
	
	
	//edge
	float z_depth = edge(texture(DEPTH_TEXTURE, SCREEN_UV).x);
	float z_pos = edge(FRAGCOORD.z);
	float z_dif = z_depth - z_pos;
	
	
	vec3 normal1 = texture(noise1_normal, world_pos.xz/noise1_scale + wave_time * noise1_speed + boat_pos).rgb;
	vec3 normal2 = texture(noise2_normal, world_pos.xz/noise2_scale + wave_time * noise2_speed + boat_pos).rgb;
	vec3 normal3 = texture(noise3_normal, world_pos.xz/noise3_scale + wave_time * noise3_speed + boat_pos).rgb;
	NORMAL_MAP = mix(normal1, normal2, 0.5);
	NORMAL_MAP = mix(NORMAL_MAP, normal3, 0.5);
	
	//depth
	float depth_texture = texture(DEPTH_TEXTURE, SCREEN_UV).r * 2.0 -1.0;
	float depth = PROJECTION_MATRIX[3][2] / (depth_texture + PROJECTION_MATRIX[2][2]);
	float depth_blend = exp((depth + VERTEX.z + depth_offset) * -beers_law);
	depth_blend = clamp(1.0 - depth_blend, 0.0, 1.0);
	float depth_blend_power = clamp(pow(depth_blend, 2.5), 0.0, 1.0);
	
	vec3 screen_color = textureLod(SCREEN_TEXTURE, SCREEN_UV, depth_blend_power * 2.5).rgb;
	vec3 depth_color = mix(color_shallow.rgb, color_deep.rgb, depth_blend_power);
	vec3 depth_rgb = mix(screen_color * depth_color, depth_color *0.25, depth_blend_power * 0.5);
	ALBEDO = depth_rgb;
	
	
	//fresnel
	float fresnel = fresnel(5.0, NORMAL, VIEW);
	ALBEDO = mix(ALBEDO, fresnel_color, fresnel);
	
	//edge
	ALBEDO = mix(edge_color, ALBEDO, step(edge_scale, z_dif));
	
	//foam
	if(texture(noise3, world_pos.xz / noise3_scale + wave_time * noise3_speed + boat_pos).r + 
		texture(noise2, world_pos.xz / noise2_scale + wave_time * noise2_speed + boat_pos).r 
		> 1.3){
		ALBEDO = vec3(1, 1, 1);
	}
	
	if(distance_fade != 0.0){
		float dist = length(world_pos - camera_pos);
		
		ALPHA = 1.0 - smoothstep(0.0, distance_fade, dist);
	}

	
	
}



//void light() {
//
//}
