using Godot;
using System;
using System.Runtime.Serialization;

public partial class BuoyancyTest : MeshInstance3D
{

    

    MeshInstance3D water;
    Image noise;

    public float noise_scale;
    public float _time = 0;
    public Vector2 noise_speed;
    public float noise_strength;

    public float offset_y = -2.5f;

    public Marker3D heightsampler_z1;
    public Marker3D heightsampler_z2;
    public Marker3D heightsampler_x1;
    public Marker3D heightsampler_x2;



    public override void _Ready()
    {
        water = GetNode<MeshInstance3D>("../Water");
        heightsampler_z1 = GetNode<Marker3D>("heightsampler_z1");
        heightsampler_z2 = GetNode<Marker3D>("heightsampler_z2");

        heightsampler_x1 = GetNode<Marker3D>("heightsampler_x1");
        heightsampler_x2 = GetNode<Marker3D>("heightsampler_x2");


        var _material = water.Mesh.SurfaceGetMaterial(0) as ShaderMaterial;

        noise_scale = (float)_material.GetShaderParameter("noise1_scale");
        noise_speed = (Vector2)_material.GetShaderParameter("noise1_speed");
        noise_strength = (float)_material.GetShaderParameter("noise1_strength");


        noise = _material.GetShaderParameter("noise1").As<NoiseTexture2D>().Noise.GetSeamlessImage(512, 512, false, false, 0.1f, true);

    }


    public override void _Process(double delta)
    {
        _time += (float)delta;

        GD.Print(noise);

        //height
        float height = getheight(GlobalPosition);
        Position = new Vector3(Position.X, height + offset_y, Position.Y);

        //rotation z
        float height_z1 = getheight(heightsampler_z1.GlobalPosition);
        float height_z2 = getheight(heightsampler_z2.GlobalPosition);
        float heightdiff_z = height_z2 - height_z1;
        Vector2 slopevect_z = new Vector2(1, heightdiff_z).Normalized();

        float newrotz = new Vector2(1, 0).AngleTo(slopevect_z);
        newrotz = Mathf.Clamp(newrotz, -0.26f, 0.26f);

        //rotation x
        float height_x1 = getheight(heightsampler_x1.GlobalPosition);
        float height_x2 = getheight(heightsampler_x2.GlobalPosition);
        float heightdiff_x = height_x2 - height_x1;
        Vector2 slopevect_x = new Vector2(1, heightdiff_x).Normalized();

        float newrotx = new Vector2(1, 0).AngleTo(slopevect_x);
        newrotz = Mathf.Clamp(newrotx, -0.35f, 0.35f);



        Rotation = new Vector3(newrotx, 0, newrotz);

    }



    public float getheight(Vector3 position)
    {
        var uv_x = Mathf.Wrap(position.X / noise_scale + _time * noise_speed.X, 0, 1);
        var uv_y = Mathf.Wrap(position.Z / noise_scale + _time * noise_speed.Y, 0, 1);

        var pixel_pos = new Vector2I(Mathf.RoundToInt(uv_x * (noise.GetWidth()-1)), Mathf.RoundToInt(uv_y * (noise.GetHeight()-1)));
        return noise.GetPixelv(pixel_pos).R * noise_strength;
    }









}
