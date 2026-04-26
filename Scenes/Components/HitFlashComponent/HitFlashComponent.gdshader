shader_type canvas_item;

uniform float percent: hint_range(0.0, 1.0, 0.1) = 0.0;

void fragment() {
	vec4 texture_color = texture(TEXTURE, UV);
	vec4 white_shadow = vec4(vec3(1.0, 1.0, 1.0), texture_color.a);

	vec4 final_color = mix(texture_color, white_shadow, percent);

	COLOR = final_color;
}
