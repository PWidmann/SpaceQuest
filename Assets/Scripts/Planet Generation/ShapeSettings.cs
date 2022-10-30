public class ShapeSettings
{
    public float planetRadius = 1;
    public bool useFirstLayerAsMask = true;
    public NoiseSettings noiseSettingsL1;
    public NoiseSettings noiseSettingsL2;

    public ShapeSettings()
    {
        noiseSettingsL1 = new NoiseSettings();
        noiseSettingsL2 = new NoiseSettings();
    }
}
