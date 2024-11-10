shader_type canvas_item;

uniform float flash_strength : hint_range(0.0, 1.0);

void fragment() {
    vec4 texColor = texture(TEXTURE, FRAGCOORD.xy / vec2(1,1));
    vec4 flashColor = vec4(1.0, 1.0, 1.0, 1.0) * flash_strength;
    COLOR = mix(texColor, flashColor, flash_strength);
}
