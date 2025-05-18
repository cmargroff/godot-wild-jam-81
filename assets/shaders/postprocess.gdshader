shader_type canvas_item;

uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;

uniform sampler2D curve_r;
uniform sampler2D new_curve_r;
uniform sampler2D curve_g;
uniform sampler2D curve_b;

uniform float time;
uniform float start_time;
uniform float changetime;

void vertex() {
	
}

void fragment() {
	
	vec3 color;
	
	color = texture(screen_texture, SCREEN_UV).rgb;
	
	//float val1 = texture(curve_r, vec2(color.r, 0.0)).r;
	//float val2 = texture(new_curve_r, vec2(color.r, 0.0)).r;
	//float relativetime = time / (start_time + changetime);
	//color.r = mix(val1, val2, relativetime);
	
	
	
	color.r = texture(curve_r, vec2(color.r, 0.0)).r;
	color.b = texture(curve_b, vec2(color.b, 0.0)).b;
	color.g = texture(curve_g, vec2(color.g, 0.0)).g;
	
	
	
	COLOR.rgb = color;
	
	
	
	
	
	
	
	
	
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
