// Decompiled with JetBrains decompiler
// Type: StardewValley.BloomComponent
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley
{
  public class BloomComponent : DrawableGameComponent
  {
    private BloomSettings settings = BloomSettings.PresetSettings[5];
    private BloomSettings targetSettings = BloomSettings.PresetSettings[5];
    private BloomSettings oldSetting = BloomSettings.PresetSettings[5];
    private BloomComponent.IntermediateBuffer showBuffer = BloomComponent.IntermediateBuffer.FinalResult;
    private SpriteBatch spriteBatch;
    private Effect bloomExtractEffect;
    private Effect brightWhiteEffect;
    private Effect bloomCombineEffect;
    private Effect gaussianBlurEffect;
    private RenderTarget2D sceneRenderTarget;
    private RenderTarget2D renderTarget1;
    private RenderTarget2D renderTarget2;
    public float hueShiftR;
    public float hueShiftG;
    public float hueShiftB;
    public float timeLeftForShifting;
    public float totalTime;
    public float shiftRate;
    public float offsetShift;
    public float shiftFade;
    public float blurLevel;
    public float saturationLevel;
    public float contrastLevel;
    public float bloomLevel;
    public float brightnessLevel;
    public float globalIntensity;
    public float globalIntensityMax;
    public float rabbitHoleTimer;
    private bool cyclingShift;

    public BloomSettings Settings
    {
      get
      {
        return this.settings;
      }
      set
      {
        this.settings = value;
      }
    }

    public BloomComponent.IntermediateBuffer ShowBuffer
    {
      get
      {
        return this.showBuffer;
      }
      set
      {
        this.showBuffer = value;
      }
    }

    public BloomComponent(Game game)
      : base(game)
    {
      if (game == null)
        throw new ArgumentNullException(nameof (game));
    }

    public void startShifting(float howLongMilliseconds, float shiftRate, float shiftFade, float globalIntensityMax, float blurShiftLevel, float saturationShiftLevel, float contrastShiftLevel, float bloomIntensityShift, float brightnessShift, float globalIntensityStart = 1f, float offsetShift = 3000f, bool cyclingShift = true)
    {
      this.timeLeftForShifting = howLongMilliseconds;
      this.totalTime = howLongMilliseconds;
      this.shiftRate = shiftRate;
      this.blurLevel = blurShiftLevel;
      this.saturationLevel = saturationShiftLevel;
      this.contrastLevel = contrastShiftLevel;
      this.bloomLevel = bloomIntensityShift;
      this.brightnessLevel = brightnessShift;
      this.Visible = true;
      this.oldSetting = new BloomSettings("old", this.settings.BloomThreshold, this.settings.BlurAmount, this.settings.BloomIntensity, this.settings.BaseIntensity, this.settings.BloomSaturation, this.settings.BaseSaturation, false);
      this.targetSettings = new BloomSettings("old", this.settings.BloomThreshold, this.settings.BlurAmount, this.settings.BloomIntensity, this.settings.BaseIntensity, this.settings.BloomSaturation, this.settings.BaseSaturation, false);
      this.cyclingShift = cyclingShift;
      this.shiftFade = shiftFade;
      this.globalIntensity = globalIntensityStart;
      this.globalIntensityMax = globalIntensityMax / 2f;
      this.offsetShift = offsetShift;
      Game1.debugOutput = ((double) howLongMilliseconds).ToString() + " " + (object) shiftRate + " " + (object) shiftFade + " " + (object) globalIntensityMax + " " + (object) blurShiftLevel + " " + (object) saturationShiftLevel + " " + (object) contrastShiftLevel + " " + (object) bloomIntensityShift + " " + (object) brightnessShift + " " + (object) globalIntensityStart + " " + (object) offsetShift;
      this.hueShiftR = 0.0f;
      this.hueShiftB = 0.0f;
      this.hueShiftG = 0.0f;
    }

    protected override void LoadContent()
    {
      this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
      this.bloomExtractEffect = this.Game.Content.Load<Effect>("BloomExtract");
      this.bloomCombineEffect = this.Game.Content.Load<Effect>("BloomCombine");
      this.gaussianBlurEffect = this.Game.Content.Load<Effect>("GaussianBlur");
      this.brightWhiteEffect = this.Game.Content.Load<Effect>("BrightWhite");
      PresentationParameters presentationParameters = this.GraphicsDevice.PresentationParameters;
      int backBufferWidth = presentationParameters.BackBufferWidth;
      int backBufferHeight = presentationParameters.BackBufferHeight;
      SurfaceFormat backBufferFormat = presentationParameters.BackBufferFormat;
      this.sceneRenderTarget = new RenderTarget2D(this.GraphicsDevice, backBufferWidth, backBufferHeight, false, backBufferFormat, presentationParameters.DepthStencilFormat, presentationParameters.MultiSampleCount, RenderTargetUsage.DiscardContents);
      int width = backBufferWidth / 2;
      int height = backBufferHeight / 2;
      this.renderTarget1 = new RenderTarget2D(this.GraphicsDevice, width, height, false, backBufferFormat, DepthFormat.None);
      this.renderTarget2 = new RenderTarget2D(this.GraphicsDevice, width, height, false, backBufferFormat, DepthFormat.None);
    }

    public void reload()
    {
      PresentationParameters presentationParameters = this.GraphicsDevice.PresentationParameters;
      int backBufferWidth = presentationParameters.BackBufferWidth;
      int backBufferHeight = presentationParameters.BackBufferHeight;
      SurfaceFormat backBufferFormat = presentationParameters.BackBufferFormat;
      this.sceneRenderTarget = new RenderTarget2D(this.GraphicsDevice, backBufferWidth, backBufferHeight, false, backBufferFormat, presentationParameters.DepthStencilFormat, presentationParameters.MultiSampleCount, RenderTargetUsage.DiscardContents);
      int width = backBufferWidth / 2;
      int height = backBufferHeight / 2;
      this.renderTarget1 = new RenderTarget2D(this.GraphicsDevice, width, height, false, backBufferFormat, DepthFormat.None);
      this.renderTarget2 = new RenderTarget2D(this.GraphicsDevice, width, height, false, backBufferFormat, DepthFormat.None);
    }

    protected override void UnloadContent()
    {
      this.sceneRenderTarget.Dispose();
      this.renderTarget1.Dispose();
      this.renderTarget2.Dispose();
    }

    public void BeginDraw()
    {
      if (!this.Visible)
        return;
      this.GraphicsDevice.SetRenderTarget(this.sceneRenderTarget);
    }

    public void tick(GameTime time)
    {
      if ((double) this.timeLeftForShifting <= 0.0)
        return;
      this.Visible = true;
      this.timeLeftForShifting = this.timeLeftForShifting - (float) time.ElapsedGameTime.Milliseconds;
      this.shiftRate = Math.Max(0.0001f, this.shiftRate + this.shiftFade * (float) time.ElapsedGameTime.Milliseconds);
      if (this.cyclingShift)
      {
        double offsetShift = (double) this.offsetShift;
        TimeSpan elapsedGameTime = time.ElapsedGameTime;
        double num1 = (double) elapsedGameTime.Milliseconds / 10.0;
        this.offsetShift = (float) (offsetShift + num1);
        this.globalIntensity = this.globalIntensityMax * (float) Math.Cos(((double) this.timeLeftForShifting - (double) this.totalTime * Math.PI * 4.0) * (2.0 * Math.PI / (double) this.totalTime)) + this.globalIntensityMax;
        float num2 = this.offsetShift * (float) Math.Sin((double) this.timeLeftForShifting * (2.0 * Math.PI) / (double) this.totalTime);
        this.targetSettings.BaseSaturation = Math.Max(1f, (float) (0.25 * (double) this.globalIntensity * ((double) this.saturationLevel * Math.Sin(((double) this.timeLeftForShifting - (double) num2 / 2.0) * (2.0 * Math.PI) / (double) this.shiftRate) + (0.25 * (double) this.globalIntensity + (double) this.saturationLevel))));
        this.targetSettings.BloomIntensity = Math.Max(0.0f, (float) (0.5 * (double) this.globalIntensity * ((double) this.bloomLevel / 2.0 * Math.Sin(((double) this.timeLeftForShifting - (double) num2 * 2.0) * (2.0 * Math.PI) / (double) this.shiftRate) + (0.5 * (double) this.globalIntensity + (double) this.bloomLevel / 2.0))));
        this.targetSettings.BlurAmount = Math.Max(0.0f, (float) (1.0 * (double) this.globalIntensity * ((double) this.blurLevel * Math.Sin((double) this.timeLeftForShifting * (2.0 * Math.PI) / ((double) this.shiftRate / 2.0))) + (1.0 * (double) this.globalIntensity + (double) this.blurLevel)));
        this.settings.BaseSaturation += (float) (((double) this.targetSettings.BaseSaturation - (double) this.settings.BaseSaturation) / 10.0);
        this.settings.BloomIntensity += (float) (((double) this.targetSettings.BloomIntensity - (double) this.settings.BloomIntensity) / 10.0);
        this.settings.BaseIntensity += (float) (((double) this.targetSettings.BaseIntensity - (double) this.settings.BaseIntensity) / 10.0);
        this.settings.BlurAmount += (float) (((double) this.targetSettings.BaseSaturation - (double) this.settings.BlurAmount) / 10.0);
        this.hueShiftR = (float) ((double) this.globalIntensity / 2.0 * (Math.Cos(((double) this.timeLeftForShifting - (double) num2 / 2.0) * (2.0 * Math.PI) / ((double) this.shiftRate / 2.0)) + 1.0) / 4.0);
        this.hueShiftG = (float) ((double) this.globalIntensity / 2.0 * (Math.Sin(((double) this.timeLeftForShifting - (double) num2 / 2.0) * (2.0 * Math.PI) / ((double) this.shiftRate / 2.0)) + 1.0) / 4.0);
        this.hueShiftB = (float) ((double) this.globalIntensity / 2.0 * (Math.Cos(((double) this.timeLeftForShifting - (double) num2 / 2.0 - (double) this.totalTime / 2.0) * (2.0 * Math.PI) / (double) this.shiftRate) + 1.0) / 4.0);
        double rabbitHoleTimer = (double) this.rabbitHoleTimer;
        elapsedGameTime = time.ElapsedGameTime;
        double milliseconds = (double) elapsedGameTime.Milliseconds;
        this.rabbitHoleTimer = (float) (rabbitHoleTimer - milliseconds);
        if ((double) this.rabbitHoleTimer <= 0.0)
        {
          this.rabbitHoleTimer = 1000f;
          Console.WriteLine("timeLeft: " + (object) this.timeLeftForShifting + " shiftRate: " + (object) this.shiftRate + " globalIntensity: " + (object) this.globalIntensity + " settings.BloomThreshold: " + (object) this.settings.BloomThreshold + " settings.BaseSaturation: " + (object) this.settings.BaseSaturation + " settings.BloomIntensity: " + (object) this.settings.BloomIntensity + " settings.BaseIntensity: " + (object) this.settings.BaseIntensity + " settings.BlurAmount: " + (object) this.settings.BlurAmount + " hueShift: " + (object) this.hueShiftR + "," + (object) this.hueShiftG + "," + (object) this.hueShiftB + " x,y: ");
        }
      }
      if ((double) this.timeLeftForShifting > 0.0)
        return;
      this.hueShiftR = 0.0f;
      this.hueShiftG = 0.0f;
      this.hueShiftB = 0.0f;
      this.settings = this.oldSetting;
      if (Game1.bloomDay && Game1.currentLocation.isOutdoors)
        this.Visible = true;
      else
        this.Visible = false;
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.settings == null)
        return;
      this.GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
      if (this.settings.brightWhiteOnly)
      {
        this.DrawFullscreenQuad((Texture2D) this.sceneRenderTarget, this.renderTarget1, this.brightWhiteEffect, BloomComponent.IntermediateBuffer.PreBloom);
      }
      else
      {
        this.bloomExtractEffect.Parameters["BloomThreshold"].SetValue(this.Settings.BloomThreshold);
        this.DrawFullscreenQuad((Texture2D) this.sceneRenderTarget, this.renderTarget1, this.bloomExtractEffect, BloomComponent.IntermediateBuffer.PreBloom);
      }
      this.SetBlurEffectParameters(1f / (float) this.renderTarget1.Width, 0.0f);
      this.DrawFullscreenQuad((Texture2D) this.renderTarget1, this.renderTarget2, this.gaussianBlurEffect, BloomComponent.IntermediateBuffer.BlurredHorizontally);
      this.SetBlurEffectParameters(0.0f, 1f / (float) this.renderTarget1.Height);
      this.DrawFullscreenQuad((Texture2D) this.renderTarget2, this.renderTarget1, this.gaussianBlurEffect, BloomComponent.IntermediateBuffer.BlurredBothWays);
      this.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);
      EffectParameterCollection parameters = this.bloomCombineEffect.Parameters;
      string index1 = "BloomIntensity";
      parameters[index1].SetValue(this.Settings.BloomIntensity);
      string index2 = "BaseIntensity";
      parameters[index2].SetValue(this.Settings.BaseIntensity);
      string index3 = "BloomSaturation";
      parameters[index3].SetValue(this.Settings.BloomSaturation);
      string index4 = "BaseSaturation";
      parameters[index4].SetValue(this.Settings.BaseSaturation);
      string index5 = "HueR";
      parameters[index5].SetValue((float) Math.Round((double) this.hueShiftR, 2));
      string index6 = "HueG";
      parameters[index6].SetValue((float) Math.Round((double) this.hueShiftG, 2));
      string index7 = "HueB";
      parameters[index7].SetValue((float) Math.Round((double) this.hueShiftB, 2));
      this.GraphicsDevice.Textures[1] = (Texture) this.sceneRenderTarget;
      Viewport viewport = this.GraphicsDevice.Viewport;
      this.DrawFullscreenQuad((Texture2D) this.renderTarget1, viewport.Width, viewport.Height, this.bloomCombineEffect, BloomComponent.IntermediateBuffer.FinalResult);
    }

    private void DrawFullscreenQuad(Texture2D texture, RenderTarget2D renderTarget, Effect effect, BloomComponent.IntermediateBuffer currentBuffer)
    {
      this.GraphicsDevice.SetRenderTarget(renderTarget);
      this.DrawFullscreenQuad(texture, renderTarget.Width, renderTarget.Height, effect, currentBuffer);
    }

    private void DrawFullscreenQuad(Texture2D texture, int width, int height, Effect effect, BloomComponent.IntermediateBuffer currentBuffer)
    {
      if (this.showBuffer < currentBuffer)
        effect = (Effect) null;
      this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, (SamplerState) null, (DepthStencilState) null, (RasterizerState) null, effect);
      this.spriteBatch.Draw(texture, new Rectangle(0, 0, width, height), Color.White);
      this.spriteBatch.End();
    }

    private void SetBlurEffectParameters(float dx, float dy)
    {
      EffectParameter parameter1 = this.gaussianBlurEffect.Parameters["SampleWeights"];
      EffectParameter parameter2 = this.gaussianBlurEffect.Parameters["SampleOffsets"];
      int count = parameter1.Elements.Count;
      float[] numArray = new float[count];
      Vector2[] vector2Array = new Vector2[count];
      numArray[0] = this.ComputeGaussian(0.0f);
      vector2Array[0] = new Vector2(0.0f);
      float num1 = numArray[0];
      for (int index = 0; index < count / 2; ++index)
      {
        float gaussian = this.ComputeGaussian((float) (index + 1));
        numArray[index * 2 + 1] = gaussian;
        numArray[index * 2 + 2] = gaussian;
        num1 += gaussian * 2f;
        float num2 = (float) (index * 2) + 1.5f;
        Vector2 vector2 = new Vector2(dx, dy) * num2;
        vector2Array[index * 2 + 1] = vector2;
        vector2Array[index * 2 + 2] = -vector2;
      }
      for (int index = 0; index < numArray.Length; ++index)
        numArray[index] /= num1;
      parameter1.SetValue(numArray);
      parameter2.SetValue(vector2Array);
    }

    private float ComputeGaussian(float n)
    {
      float blurAmount = this.Settings.BlurAmount;
      return (float) (1.0 / Math.Sqrt(2.0 * Math.PI * (double) blurAmount) * Math.Exp(-((double) n * (double) n) / (2.0 * (double) blurAmount * (double) blurAmount)));
    }

    public enum IntermediateBuffer
    {
      PreBloom,
      BlurredHorizontally,
      BlurredBothWays,
      FinalResult,
    }
  }
}
