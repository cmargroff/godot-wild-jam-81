using Godot;

public partial class WeatherType : Resource
{
    [ExportGroup("Water Params")]
    [Export]
    [Tweenable, WaterMaterial]
    public float noise1_scale = 400;
    [Export]
    [Tweenable, WaterMaterial]
    public float noise2_scale = 200;
    [Export]
    [Tweenable, WaterMaterial]
    public float noise3_scale = 50;
    [Export]
    [Tweenable, WaterMaterial]
    public Vector2 noise1_speed = new(0.01f, 0);
    [Export]
    [Tweenable, WaterMaterial]
    public Vector2 noise2_speed = new(0.01f, 0.01f);
    [Export]
    [Tweenable, WaterMaterial]
    public Vector2 noise3_speed = new(-0.01f, -0.03f);
    [Export]
    [Tweenable, WaterMaterial]
    public float noise1_strength = 10;
    [Export]
    [Tweenable, WaterMaterial]
    public float noise2_strength = 2;
    [Export]
    [Tweenable, WaterMaterial]
    public float noise3_strength = 0.3f;
    [Export]
    [Tweenable, WaterMaterial]
    public Color fresnel_color = new(0.2f, 0.63f, 0.47f);
    [Export]
    [Tweenable, WaterMaterial]
    public Color color_deep = new(0, 0.6f, 0.47f);
    [Export]
    [Tweenable, WaterMaterial]
    public Color color_shallow = new(0, 0.81f, 0.6f);

    [ExportGroup("Sky Params")]
    [Export(PropertyHint.Range, "0.0, 1.0")] //TODO: verify that this doesn't snap
    [Tweenable, SkyMaterial]
    public float clouds_density = 0.4f;
    [Export]
    [Tweenable, SkyMaterial]
    public Color clouds_light_color = new(1, 1, 1);
    [Export]
    [Tweenable, SkyMaterial]
    public Color top_color = new(0.73f, 0.92f, 1);
    [Export]
    [Tweenable, SkyMaterial]
    public Color bottom_color = new(0.32f, 0.74f, 0.92f);
    [Export]
    [Tweenable, SkyMaterial]
    public Color sun_scatter = new(0.3f, 0.3f, 0.3f);


    [ExportGroup("Sun Params")]
    [Export]
    [Tweenable, SunMaterial]
    public Color light_color = new(1, 1, 1);
    [Export]
    [Tweenable, SunMaterial]
    public float light_energy = 1.0f;

    [ExportGroup("Rain Params")]
    [Export]
    public bool is_raining = false;
    [Export]
    [Tweenable]
    public int rain_amount = 1000;

    [Export]
    public CurveTexture curve_r;
    [Export]
    public CurveTexture curve_g;
    [Export]
    public CurveTexture curve_b;

    public WeatherType Clone()
    {
        return (WeatherType)MemberwiseClone();
    }
}
