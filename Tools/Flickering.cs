using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Rapid_Prototype_1
{
    public class Flickering
    {
        GraphicsDevice device;
        SpriteBatch spriteBatch;
        private float _TimeAmount;
        Effect bloomExtractEffect;
        Effect bloomCombineEffect;
        Effect gaussianBlurEffect;
        RenderTarget2D renderTarget1;
        RenderTarget2D renderTarget2;
        public FlickeringSettings Settings { get { return settings; } set { settings = value; } }
        FlickeringSettings settings = FlickeringSettings.PresetSettings[6];

        public Flickering(GraphicsDevice graphics, SpriteBatch passedSpriteBatch)
        {
            device = graphics;
            spriteBatch = passedSpriteBatch;
            _TimeAmount = 0.0f;
        }
        public void LoadContent(ContentManager Content, PresentationParameters pp)
        {
            bloomExtractEffect = Content.Load<Effect>("SHADER/FlickeringExtract");
            bloomCombineEffect = Content.Load<Effect>("SHADER/FlickeringCombine");
            gaussianBlurEffect = Content.Load<Effect>("SHADER/GaussianBlur");

            int width = pp.BackBufferWidth, height = pp.BackBufferHeight;
            SurfaceFormat format = pp.BackBufferFormat;

            // Create two rendertargets for the bloom processing. These are half the size of the backbuffer, in order to minimize fillrate costs. Reducing the resolution in this way doesn't hurt quality, because we are going to be blurring the bloom images in any case.
            width /= 2;
            height /= 2;

            renderTarget1 = new RenderTarget2D(device, width, height, false, format, DepthFormat.None);
            renderTarget2 = new RenderTarget2D(device, width, height, false, format, DepthFormat.None);
        }
        public void UnloadContent()
        {
            renderTarget1.Dispose();
            renderTarget2.Dispose();
        }
        public void Draw(RenderTarget2D sourceRenderTarget, RenderTarget2D destRenderTarget)
        {

            bloomExtractEffect.Parameters["BloomThreshold"].SetValue(Settings.BloomThreshold);
            DrawFullscreenQuad(sourceRenderTarget, renderTarget1, bloomExtractEffect);

            SetBlurEffectParameters(1.0f / (float)renderTarget1.Width, 0);
            DrawFullscreenQuad(renderTarget1, renderTarget2, gaussianBlurEffect);

            SetBlurEffectParameters(0, 1.0f / (float)renderTarget1.Height);
            DrawFullscreenQuad(renderTarget2, renderTarget1, gaussianBlurEffect);

            device.SetRenderTarget(destRenderTarget);

            EffectParameterCollection parameters = bloomCombineEffect.Parameters;

            parameters["BloomIntensity"].SetValue(Settings.BloomIntensity);
            parameters["BaseIntensity"].SetValue(Settings.BaseIntensity);
            parameters["BloomSaturation"].SetValue(Settings.BloomSaturation);
            parameters["BaseSaturation"].SetValue(Settings.BaseSaturation);

            bloomCombineEffect.Parameters["BaseTexture"].SetValue(sourceRenderTarget);

            Viewport viewport = device.Viewport;

            DrawFullscreenQuad(renderTarget1, viewport.Width, viewport.Height, bloomCombineEffect);
        }
        void DrawFullscreenQuad(Texture2D texture, RenderTarget2D renderTarget, Effect effect)
        {
            device.SetRenderTarget(renderTarget);
            DrawFullscreenQuad(texture, renderTarget.Width, renderTarget.Height, effect);
        }
        void DrawFullscreenQuad(Texture2D texture, int width, int height, Effect effect)
        {
            device.Clear(Color.TransparentBlack); 
            spriteBatch.Begin(0, BlendState.AlphaBlend, null, null, null, effect);
            spriteBatch.Draw(texture, new Rectangle(0, 0, width, height), Color.White);
            spriteBatch.End();
        }
        void SetBlurEffectParameters(float dx, float dy)
        {
            // Look up the sample weight and offset effect parameters.
            EffectParameter weightsParameter, offsetsParameter;

            weightsParameter = gaussianBlurEffect.Parameters["SampleWeights"];
            offsetsParameter = gaussianBlurEffect.Parameters["SampleOffsets"];

            // Look up how many samples our gaussian blur effect supports.
            int sampleCount = weightsParameter.Elements.Count;

            // Create temporary arrays for computing our filter settings.
            float[] sampleWeights = new float[sampleCount];
            Vector2[] sampleOffsets = new Vector2[sampleCount];

            // The first sample always has a zero offset.
            sampleWeights[0] = ComputeGaussian(0);
            sampleOffsets[0] = new Vector2(0);

            // Maintain a sum of all the weighting values.
            float totalWeights = sampleWeights[0];

            // Add pairs of additional sample taps, positioned along a line in both directions from the center.
            for (int i = 0; i < sampleCount / 2; i++)
            {
                // Store weights for the positive and negative taps.
                float weight = ComputeGaussian(i + 1);

                sampleWeights[i * 2 + 1] = weight;
                sampleWeights[i * 2 + 2] = weight;

                totalWeights += weight * 2;

                // To get the maximum amount of blurring from a limited number of pixel shader samples, we take advantage of the bilinear filtering
                // hardware inside the texture fetch unit. If we position our texture coordinates exactly halfway between two texels, the filtering unit
                // will average them for us, giving two samples for the price of one. This allows us to step in units of two texels per sample, rather
                // than just one at a time. The 1.5 offset kicks things off by positioning us nicely in between two texels.
                float sampleOffset = i * 2 + 1.5f;

                Vector2 delta = new Vector2(dx, dy) * sampleOffset;

                // Store texture coordinate offsets for the positive and negative taps.
                sampleOffsets[i * 2 + 1] = delta;
                sampleOffsets[i * 2 + 2] = -delta;
            }

            // Normalize the list of sample weightings, so they will always sum to one.
            for (int i = 0; i < sampleWeights.Length; i++)
            {
                sampleWeights[i] /= totalWeights;
            }

            weightsParameter.SetValue(sampleWeights);  // Tell the effect about our new filter settings.
            offsetsParameter.SetValue(sampleOffsets);
        }
        float ComputeGaussian(float n)
        {
            float theta = Settings.BlurAmount;
            return (float)((1.0 / Math.Sqrt(2 * Math.PI * theta)) *
                            Math.Exp(-(n * n) / (2 * theta * theta)));
        }
    }

}
