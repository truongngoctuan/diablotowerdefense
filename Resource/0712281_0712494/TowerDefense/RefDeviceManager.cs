//http://tieba.baidu.com/f?kz=396369725

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Security;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KkunFGame
{
    public class ReferenceGraphicsDeviceManager
    : IGraphicsDeviceService, IDisposable, IGraphicsDeviceManager
    {

        // Events
        public event EventHandler DeviceCreated;
        public event EventHandler DeviceDisposing;
        public event EventHandler DeviceReset;
        public event EventHandler DeviceResetting;
        public event EventHandler Disposed;
        public event EventHandler<PreparingDeviceSettingsEventArgs> PreparingDeviceSettings;

        public static readonly int DefaultBackBufferHeight = 800;
        public static readonly int DefaultBackBufferWidth = 600;
        public static readonly SurfaceFormat[] ValidAdapterFormats = new SurfaceFormat[] {
 SurfaceFormat.Bgr32, SurfaceFormat.Bgr555, SurfaceFormat.Bgr565, SurfaceFormat.Bgra1010102
 };
        public static readonly SurfaceFormat[] ValidBackBufferFormats = new SurfaceFormat[] {
 SurfaceFormat.Bgr565, SurfaceFormat.Bgr555, SurfaceFormat.Bgra5551, SurfaceFormat.Bgr32,
 SurfaceFormat.Color, SurfaceFormat.Bgra1010102
 };
        public static readonly DeviceType[] ValidDeviceTypes = {
 DeviceType.Hardware
 };

        // Methods
        public ReferenceGraphicsDeviceManager(Game game)
        {
            this.synchronizeWithVerticalRetrace = true;
            this.backBufferFormat = SurfaceFormat.Color;
            this.depthStencilFormat = DepthFormat.Depth24;
            this.backBufferWidth = GraphicsDeviceManager.DefaultBackBufferWidth;
            this.backBufferHeight = GraphicsDeviceManager.DefaultBackBufferHeight;
            this.minimumVertexShaderProfile = ShaderProfile.VS_1_1;
            if (game == null)
            {
                throw new ArgumentNullException("Resources.GameCannotBeNull");
            }
            this.game = game;
            if (game.Services.GetService(typeof(IGraphicsDeviceManager)) != null)
            {
                throw new ArgumentException("Resources.GraphicsDeviceManagerAlreadyPresent");
            }
            game.Services.AddService(typeof(IGraphicsDeviceManager), this);
            game.Services.AddService(typeof(IGraphicsDeviceService), this);
            game.Window.ClientSizeChanged += new EventHandler(this.GameWindowClientSizeChanged);
            game.Window.ScreenDeviceNameChanged += new EventHandler(this.GameWindowScreenDeviceNameChanged);
        }
        private void AddDevices(bool anySuitableDevice, List<GraphicsDeviceInformation> foundDevices)
        {
            IntPtr ptr1 = this.game.Window.Handle;
            using (IEnumerator<GraphicsAdapter> enumerator1 = GraphicsAdapter.Adapters.GetEnumerator())
            {
                while (enumerator1.MoveNext())
                {
                    GraphicsAdapter adapter1 = enumerator1.Current;
                    if (anySuitableDevice || this.IsWindowOnAdapter(ptr1, adapter1))
                    {
                        foreach (DeviceType type1 in GraphicsDeviceManager.ValidDeviceTypes)
                        {
                            try
                            {
                                GraphicsDeviceCapabilities capabilities1 = adapter1.GetCapabilities(type1);
                                if ((capabilities1.DeviceCapabilities.IsDirect3D9Driver && (capabilities1.MaxPixelShaderProfile >= this.MinimumPixelShaderProfile)) && (capabilities1.MaxVertexShaderProfile >= this.MinimumVertexShaderProfile))
                                {


                                    //CreateOptions options1 = CreateOptions.None;
                                    //if (capabilities1.DeviceCapabilities.SupportsHardwareTransformAndLight) {
                                    // options1 |= CreateOptions.HardwareVertexProcessing;
                                    //} else {
                                    // options1 |= CreateOptions.SoftwareVertexProcessing;
                                    //}
                                    GraphicsDeviceInformation information1 = new GraphicsDeviceInformation();
                                    information1.Adapter = adapter1;
                                    information1.DeviceType = type1;
                                    //information1.CreationOptions = options1;
                                    information1.PresentationParameters.DeviceWindowHandle = IntPtr.Zero;
                                    information1.PresentationParameters.EnableAutoDepthStencil = true;
                                    information1.PresentationParameters.BackBufferCount = 1;
                                    information1.PresentationParameters.PresentOptions = PresentOptions.None;
                                    information1.PresentationParameters.SwapEffect = SwapEffect.Discard;
                                    information1.PresentationParameters.FullScreenRefreshRateInHz = 0;
                                    information1.PresentationParameters.MultiSampleQuality = 0;
                                    information1.PresentationParameters.MultiSampleType = MultiSampleType.None;
                                    information1.PresentationParameters.IsFullScreen = this.IsFullScreen;
                                    information1.PresentationParameters.PresentationInterval = this.SynchronizeWithVerticalRetrace ? PresentInterval.One : PresentInterval.Immediate;
                                    for (int num1 = 0; num1 < GraphicsDeviceManager.ValidAdapterFormats.Length; num1++)
                                    {
                                        this.AddDevices(adapter1, type1, adapter1.CurrentDisplayMode, information1, foundDevices);
                                        if (this.isFullScreen)
                                        {
                                            foreach (DisplayMode mode1 in adapter1.SupportedDisplayModes[GraphicsDeviceManager.ValidAdapterFormats[num1]])
                                            {
                                                if ((mode1.Width >= 640) && (mode1.Height >= 480))
                                                {
                                                    this.AddDevices(adapter1, type1, mode1, information1, foundDevices);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (DeviceNotSupportedException)
                            {
                            }
                        }
                    }
                }
            }
        }
        private void AddDevices(
        GraphicsAdapter adapter, DeviceType deviceType,
        DisplayMode mode, GraphicsDeviceInformation baseDeviceInfo,
        List<GraphicsDeviceInformation> foundDevices
        )
        {
            for (int num1 = 0; num1 < GraphicsDeviceManager.ValidBackBufferFormats.Length; num1++)
            {
                SurfaceFormat format1 = GraphicsDeviceManager.ValidBackBufferFormats[num1];
                if (adapter.CheckDeviceType(deviceType, mode.Format, format1, this.IsFullScreen))
                {
                    GraphicsDeviceInformation information1 = baseDeviceInfo.Clone();
                    if (this.IsFullScreen)
                    {
                        information1.PresentationParameters.BackBufferWidth = mode.Width;
                        information1.PresentationParameters.BackBufferHeight = mode.Height;
                        information1.PresentationParameters.FullScreenRefreshRateInHz = mode.RefreshRate;
                    }
                    else if (this.useResizedBackBuffer)
                    {
                        information1.PresentationParameters.BackBufferWidth = this.resizedBackBufferWidth;
                        information1.PresentationParameters.BackBufferHeight = this.resizedBackBufferHeight;
                    }
                    else
                    {
                        information1.PresentationParameters.BackBufferWidth = this.PreferredBackBufferWidth;
                        information1.PresentationParameters.BackBufferHeight = this.PreferredBackBufferHeight;
                    }
                    information1.PresentationParameters.BackBufferFormat = format1;
                    information1.PresentationParameters.AutoDepthStencilFormat = this.PreferredDepthStencilFormat;


                    if (this.PreferMultiSampling)
                    {
                        for (int num2 = 0; num2 < multiSampleTypes.Length; num2++)
                        {
                            int num3 = 0;
                            MultiSampleType type1 = multiSampleTypes[num2];
                            if (adapter.CheckDeviceMultiSampleType(deviceType, format1, this.IsFullScreen, type1, out num3))
                            {
                                GraphicsDeviceInformation information2 = information1.Clone();
                                information2.PresentationParameters.MultiSampleType = type1;
                                if (num3 > 0)
                                {
                                    information2.PresentationParameters.MultiSampleQuality = num3 - 1;
                                }
                                if (!foundDevices.Contains(information2))
                                {
                                    foundDevices.Add(information2);
                                }
                                break;
                            }
                        }
                    }
                    else if (!foundDevices.Contains(information1))
                    {
                        foundDevices.Add(information1);
                    }
                }
            }
        }
        public void ApplyChanges()
        {
            if ((this.device == null) || this.isDeviceDirty)
            {
                this.ChangeDevice(false);
            }
        }
        protected virtual bool CanResetDevice(GraphicsDeviceInformation newDeviceInfo)
        {
            if (this.device.CreationParameters.Adapter != newDeviceInfo.Adapter)
            {
                return false;
            }
            if (this.device.CreationParameters.DeviceType != newDeviceInfo.DeviceType)
            {
                return false;
            }
            //if (this.device.CreationParameters.CreationOptions != newDeviceInfo.CreationOptions) {
            // return false;
            //}
            return true;
        }
        private void ChangeDevice(bool forceCreate)
        {
            if (this.game == null)
            {
                throw new InvalidOperationException("Resources.GraphicsComponentNotAttachedToGame");
            }
            this.CheckForAvailableSupportedHardware();
            this.inDeviceTransition = true;
            string text1 = this.game.Window.ScreenDeviceName;
            int num1 = this.game.Window.ClientBounds.Width;
            int num2 = this.game.Window.ClientBounds.Height;
            bool flag1 = false;
            try
            {
                GraphicsDeviceInformation information1 = this.FindBestDevice(forceCreate);
                this.game.Window.BeginScreenDeviceChange(information1.PresentationParameters.IsFullScreen);
                flag1 = true;
                bool flag2 = true;
                if (!forceCreate && (this.device != null))
                {
                    this.OnPreparingDeviceSettings(this, new PreparingDeviceSettingsEventArgs(information1));
                    if (this.CanResetDevice(information1))
                    {
                        try
                        {
                            GraphicsDeviceInformation information2 = information1.Clone();
                            this.MassagePresentParameters(information1.PresentationParameters);
                            this.ValidateGraphicsDeviceInformation(information1);
                            this.device.Reset(information2.PresentationParameters);
                            flag2 = false;
                        }
                        catch
                        {
                        }
                    }
                }
                if (flag2)
                {
                    this.CreateDevice(information1);
                }
                PresentationParameters parameters1 = this.device.PresentationParameters;
                text1 = this.device.CreationParameters.Adapter.DeviceName;
                this.isReallyFullScreen = parameters1.IsFullScreen;
                if (parameters1.BackBufferWidth != 0)
                {
                    num1 = parameters1.BackBufferWidth;
                }
                if (parameters1.BackBufferHeight != 0)
                {
                    num2 = parameters1.BackBufferHeight;
                }
                this.isDeviceDirty = false;
            }
            finally
            {
                if (flag1)
                {
                    this.game.Window.EndScreenDeviceChange(text1, num1, num2);
                }
                this.inDeviceTransition = false;
            }
        }
        private void CheckForAvailableSupportedHardware()
        {
            bool flag1 = false;
            bool flag2 = false;
            foreach (GraphicsAdapter adapter1 in GraphicsAdapter.Adapters)
            {




                if (adapter1.IsDeviceTypeAvailable(DeviceType.Hardware))
                {
                    flag1 = true;
                    GraphicsDeviceCapabilities capabilities1 = adapter1.GetCapabilities(DeviceType.Hardware);
                    if (((capabilities1.MaxPixelShaderProfile != ShaderProfile.Unknown) && (capabilities1.MaxPixelShaderProfile >= ShaderProfile.PS_1_1)) && capabilities1.DeviceCapabilities.IsDirect3D9Driver)
                    {
                        flag2 = true;
                        break;
                    }
                }
            }
            if (!flag1)
            {
                if (GetSystemMetrics(0x1000) != 0)
                {
                    throw new NoSuitableGraphicsDeviceException("Resources.NoDirect3DAccelerationRemoteDesktop");
                }
                throw new NoSuitableGraphicsDeviceException("Resources.NoDirect3DAcceleration");
            }
            if (!flag2)
            {
                // HACK: Do not throw this exception. Allows the game to run without
                // a suitable graphics card installed with the reference rasterizer
                //throw new NoSuitableGraphicsDeviceException("Resources.NoPixelShader11OrDDI9Support");
            }
        }
        private void CreateDevice(GraphicsDeviceInformation newInfo)
        {
            if (this.device != null)
            {
                this.device.Dispose();
                this.device = null;
            }
            this.OnPreparingDeviceSettings(this, new PreparingDeviceSettingsEventArgs(newInfo));
            this.MassagePresentParameters(newInfo.PresentationParameters);
            try
            {
                this.ValidateGraphicsDeviceInformation(newInfo);
                GraphicsDevice device1 = new GraphicsDevice(newInfo.Adapter, newInfo.DeviceType, this.game.Window.Handle, newInfo.PresentationParameters);
                this.device = device1;
                this.device.DeviceResetting += new EventHandler(this.HandleDeviceResetting);
                this.device.DeviceReset += new EventHandler(this.HandleDeviceReset);
                this.device.DeviceLost += new EventHandler(this.HandleDeviceLost);
                this.device.Disposing += new EventHandler(this.HandleDisposing);
            }
            catch (DeviceNotSupportedException exception1)
            {
                throw new NoSuitableGraphicsDeviceException("Resources.Direct3DNotAvailable", exception1);
            }
            catch (DriverInternalErrorException exception2)
            {
                throw new NoSuitableGraphicsDeviceException("Resources.Direct3DInternalDriverError", exception2);
            }
            catch (ArgumentException exception3)
            {
                throw new NoSuitableGraphicsDeviceException("Resources.Direct3DInvalidCreateParameters", exception3);
            }
            catch (Exception exception4)
            {
                throw new NoSuitableGraphicsDeviceException("Resources.Direct3DCreateError", exception4);
            }
            this.OnDeviceCreated(this, EventArgs.Empty);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.game != null)
                {
                    if (this.game.Services.GetService(typeof(IGraphicsDeviceService)) == this)
                    {
                        this.game.Services.RemoveService(typeof(IGraphicsDeviceService));
                    }
                    this.game.Window.ClientSizeChanged -= new EventHandler(this.GameWindowClientSizeChanged);
                    this.game.Window.ScreenDeviceNameChanged -= new EventHandler(this.GameWindowScreenDeviceNameChanged);
                }
                if (this.device != null)
                {
                    this.device.Dispose();
                    this.device = null;
                }
                if (this.Disposed != null)
                {
                    this.Disposed(this, EventArgs.Empty);
                }
            }
        }
        private bool EnsureDevice()
        {
            if (this.device == null)
            {
                return false;
            }
            if (this.isReallyFullScreen && !this.game.IsActive)
            {




                return false;
            }
            switch (this.device.GraphicsDeviceStatus)
            {
                case GraphicsDeviceStatus.Lost:
                    Thread.Sleep((int)deviceLostSleepTime.TotalMilliseconds);
                    return false;

                case GraphicsDeviceStatus.NotReset:
                    Thread.Sleep((int)deviceLostSleepTime.TotalMilliseconds);
                    try
                    {
                        this.ChangeDevice(false);
                    }
                    catch (DeviceLostException)
                    {
                        return false;
                    }
                    catch
                    {
                        this.ChangeDevice(true);
                    }
                    break;
            }
            return true;
        }
        protected virtual GraphicsDeviceInformation FindBestDevice(bool anySuitableDevice)
        {
            // HACK: Do not select the best device. Instead, just throw out the
            // reference rasterizer as the selected device.
            GraphicsDeviceInformation gdi = new GraphicsDeviceInformation();
            gdi.DeviceType = DeviceType.Reference;
            //gdi.CreationOptions = CreateOptions.SoftwareVertexProcessing;
            return gdi;
            //return this.FindBestPlatformDevice(anySuitableDevice);
        }
        private GraphicsDeviceInformation FindBestPlatformDevice(bool anySuitableDevice)
        {
            List<GraphicsDeviceInformation> list1 = new List<GraphicsDeviceInformation>();
            this.AddDevices(anySuitableDevice, list1);
            if ((list1.Count == 0) && this.PreferMultiSampling)
            {
                this.PreferMultiSampling = false;
                this.AddDevices(anySuitableDevice, list1);
            }
            if (list1.Count == 0)
            {
                throw new NoSuitableGraphicsDeviceException("Resources.NoCompatibleDevices");
            }
            this.RankDevices(list1);
            if (list1.Count == 0)
            {
                throw new NoSuitableGraphicsDeviceException("Resources.NoCompatibleDevicesAfterRanking");
            }
            return list1[0];
        }
        private void GameWindowClientSizeChanged(object sender, EventArgs e)
        {
            if (!this.inDeviceTransition && ((this.game.Window.ClientBounds.Height != 0) || (this.game.Window.ClientBounds.Width != 0)))
            {
                this.resizedBackBufferWidth = this.game.Window.ClientBounds.Width;
                this.resizedBackBufferHeight = this.game.Window.ClientBounds.Height;
                this.useResizedBackBuffer = true;
                this.ChangeDevice(false);
            }
        }
        private void GameWindowScreenDeviceNameChanged(object sender, EventArgs e)
        {
            if (!this.inDeviceTransition)
            {
                this.ChangeDevice(false);
            }
        }
        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(uint smIndex);
        private void HandleDeviceLost(object sender, EventArgs e) { }
        private void HandleDeviceReset(object sender, EventArgs e)
        {
            this.OnDeviceReset(this, EventArgs.Empty);
        }
        private void HandleDeviceResetting(object sender, EventArgs e)
        {
            this.OnDeviceResetting(this, EventArgs.Empty);
        }
        private void HandleDisposing(object sender, EventArgs e)
        {
            this.OnDeviceDisposing(this, EventArgs.Empty);
        }
        private bool IsWindowOnAdapter(IntPtr windowHandle, GraphicsAdapter adapter)
        {
            return (ScreenFromAdapter(adapter) == ScreenFromHandle(windowHandle));
        }
        private void MassagePresentParameters(PresentationParameters pp)
        {
            bool flag1 = pp.BackBufferWidth == 0;
            bool flag2 = pp.BackBufferHeight == 0;
            if (!pp.IsFullScreen)
            {
                RECT rect1;
                IntPtr ptr1 = pp.DeviceWindowHandle;
                if (ptr1 == IntPtr.Zero)
                {
                    if (this.game == null)
                    {
                        throw new InvalidOperationException("Resources.GraphicsComponentNotAttachedToGame");




                    }
                    ptr1 = this.game.Window.Handle;
                }
                GetClientRect(ptr1, out rect1);
                if (flag1 && (rect1.Right == 0))
                {
                    pp.BackBufferWidth = 1;
                }
                if (flag2 && (rect1.Bottom == 0))
                {
                    pp.BackBufferHeight = 1;
                }
            }
        }
        bool IGraphicsDeviceManager.BeginDraw()
        {
            if (!this.EnsureDevice())
            {
                return false;
            }
            this.beginDrawOk = true;
            return true;
        }
        void IGraphicsDeviceManager.CreateDevice()
        {
            this.ChangeDevice(true);
        }
        void IGraphicsDeviceManager.EndDraw()
        {
            if (this.beginDrawOk && (this.device != null))
            {
                try
                {
                    this.device.Present();
                }
                catch (DeviceLostException)
                {
                }
                catch (DeviceNotResetException)
                {
                }
                catch (DriverInternalErrorException)
                {
                }
            }
        }
        protected virtual void OnDeviceCreated(object sender, EventArgs args)
        {
            if (this.deviceCreated != null)
            {
                this.deviceCreated(sender, args);
            }
        }
        protected virtual void OnDeviceDisposing(object sender, EventArgs args)
        {
            if (this.deviceDisposing != null)
            {
                this.deviceDisposing(sender, args);
            }
        }
        protected virtual void OnDeviceReset(object sender, EventArgs args)
        {
            if (this.deviceReset != null)
            {
                this.deviceReset(sender, args);
            }
        }
        protected virtual void OnDeviceResetting(object sender, EventArgs args)
        {
            if (this.deviceResetting != null)
            {
                this.deviceResetting(sender, args);
            }
        }
        protected virtual void OnPreparingDeviceSettings(
        object sender, PreparingDeviceSettingsEventArgs args
        )
        {
            if (this.PreparingDeviceSettings != null)
            {
                this.PreparingDeviceSettings(sender, args);
            }
        }
        protected virtual void RankDevices(List<GraphicsDeviceInformation> foundDevices)
        {
            this.RankDevicesPlatform(foundDevices);
        }
        private void RankDevicesPlatform(List<GraphicsDeviceInformation> foundDevices)
        {
            int num1 = 0;
            while (num1 < foundDevices.Count)
            {
                DeviceType type1 = foundDevices[num1].DeviceType;
                GraphicsAdapter adapter1 = foundDevices[num1].Adapter;
                PresentationParameters parameters1 = foundDevices[num1].PresentationParameters;
                if (!adapter1.CheckDeviceFormat(type1, adapter1.CurrentDisplayMode.Format, TextureUsage.AutoGenerateMipMap, QueryUsages.PostPixelShaderBlending, ResourceType.Texture2D, parameters1.BackBufferFormat))
                {
                    foundDevices.RemoveAt(num1);
                }
                else
                {
                    num1++;
                }
            }
            foundDevices.Sort(new GraphicsDeviceInformationComparer(this));
        }
        void IDisposable.Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void ToggleFullScreen()
        {
            this.IsFullScreen = !this.IsFullScreen;
            this.ChangeDevice(false);
        }
        private void ValidateGraphicsDeviceInformation(GraphicsDeviceInformation devInfo)
        {
            SurfaceFormat format3;
            GraphicsAdapter adapter1 = devInfo.Adapter;
            DeviceType type1 = devInfo.DeviceType;
            bool flag1 = devInfo.PresentationParameters.EnableAutoDepthStencil;
            DepthFormat format1 = devInfo.PresentationParameters.AutoDepthStencilFormat;
            SurfaceFormat format2 = devInfo.PresentationParameters.BackBufferFormat;
            int num1 = devInfo.PresentationParameters.BackBufferWidth;
            int num2 = devInfo.PresentationParameters.BackBufferHeight;
            PresentationParameters parameters1 = devInfo.PresentationParameters;




            SurfaceFormat format4 = parameters1.BackBufferFormat;
            if (!parameters1.IsFullScreen)
            {
                format3 = adapter1.CurrentDisplayMode.Format;
                if (SurfaceFormat.Unknown == parameters1.BackBufferFormat)
                {
                    format4 = format3;
                }
            }
            else
            {
                SurfaceFormat format5 = parameters1.BackBufferFormat;
                if (format5 != SurfaceFormat.Color)
                {
                    if (format5 != SurfaceFormat.Bgra5551)
                    {
                        format3 = parameters1.BackBufferFormat;
                    }
                    else
                    {
                        format3 = SurfaceFormat.Bgr555;
                    }
                }
                else
                {
                    format3 = SurfaceFormat.Bgr32;
                }
            }
            if (-1 == Array.IndexOf<SurfaceFormat>(GraphicsDeviceManager.ValidBackBufferFormats, format4))
            {
                throw new ArgumentException("Resources.ValidateBackBufferFormatIsInvalid");
            }
            if (!adapter1.CheckDeviceType(type1, format3, parameters1.BackBufferFormat, parameters1.IsFullScreen))
            {
                throw new ArgumentException("Resources.ValidateDeviceType");
            }
            if ((parameters1.BackBufferCount < 0) || (parameters1.BackBufferCount > 3))
            {
                throw new ArgumentException("Resources.ValidateBackBufferCount");
            }
            if ((parameters1.BackBufferCount > 1) && (parameters1.SwapEffect == SwapEffect.Copy))
            {
                throw new ArgumentException("Resources.ValidateBackBufferCountSwapCopy");
            }
            switch (parameters1.SwapEffect)
            {
                case SwapEffect.Discard:
                case SwapEffect.Flip:
                case SwapEffect.Copy:
                    {
                        int num3;
                        if (!adapter1.CheckDeviceMultiSampleType(type1, format4, parameters1.IsFullScreen, parameters1.MultiSampleType, out num3))
                        {
                            throw new ArgumentException("Resources.ValidateMultiSampleTypeInvalid");
                        }
                        if (parameters1.MultiSampleQuality >= num3)
                        {
                            throw new ArgumentException("Resources.ValidateMultiSampleQualityInvalid");
                        }
                        if ((parameters1.MultiSampleType != MultiSampleType.None) && (parameters1.SwapEffect != SwapEffect.Discard))
                        {
                            throw new ArgumentException("Resources.ValidateMultiSampleSwapEffect");
                        }
                        if (((parameters1.PresentOptions & PresentOptions.DiscardDepthStencil) != PresentOptions.None) && !parameters1.EnableAutoDepthStencil)
                        {
                            throw new ArgumentException("Resources.ValidateAutoDepthStencilMismatch");
                        }
                        if (parameters1.EnableAutoDepthStencil)
                        {
                            if (!adapter1.CheckDeviceFormat(type1, format3, TextureUsage.AutoGenerateMipMap, QueryUsages.None, ResourceType.DepthStencilBuffer, parameters1.AutoDepthStencilFormat))
                            {
                                throw new ArgumentException("Resources.ValidateAutoDepthStencilFormatInvalid");
                            }
                            if (!adapter1.CheckDepthStencilMatch(type1, format3, format4, parameters1.AutoDepthStencilFormat))
                            {
                                throw new ArgumentException("Resources.ValidateAutoDepthStencilFormatIncompatible");
                            }
                        }
                        if (!parameters1.IsFullScreen)
                        {
                            if (parameters1.FullScreenRefreshRateInHz != 0)
                            {
                                throw new ArgumentException("Resources.ValidateRefreshRateInWindow");
                            }
                            switch (parameters1.PresentationInterval)
                            {
                                case PresentInterval.Default:
                                case PresentInterval.One:
                                    return;

                                case PresentInterval.Immediate:
                                    return;
                            }
                            throw new ArgumentException("Resources.ValidatePresentationIntervalInWindow");
                        }
                        if (parameters1.FullScreenRefreshRateInHz == 0)
                        {
                            throw new ArgumentException("Resources.ValidateRefreshRateInFullScreen");




                        }
                        GraphicsDeviceCapabilities capabilities1 = adapter1.GetCapabilities(type1);
                        switch (parameters1.PresentationInterval)
                        {
                            case PresentInterval.Default:
                            case PresentInterval.One:
                            case PresentInterval.Immediate:
                                goto Label_02E5;

                            case PresentInterval.Two:
                            case PresentInterval.Three:
                            case PresentInterval.Four:
                                if ((capabilities1.PresentInterval & parameters1.PresentationInterval) == PresentInterval.Default)
                                {
                                    throw new ArgumentException("Resources.ValidatePresentationIntervalIncompatibleInFullScreen");
                                }
                                goto Label_02E5;
                        }
                        break;
                    }
                default:
                    throw new ArgumentException("Resources.ValidateSwapEffectInvalid");
            }
            throw new ArgumentException("Resources.ValidatePresentationIntervalInFullScreen");
        Label_02E5:
            if (parameters1.IsFullScreen)
            {
                if ((parameters1.BackBufferWidth == 0) || (parameters1.BackBufferHeight == 0))
                {
                    throw new ArgumentException("Resources.ValidateBackBufferDimsFullScreen");
                }
                bool flag2 = true;
                bool flag3 = false;
                DisplayMode mode1 = adapter1.CurrentDisplayMode;
                if (((mode1.Format != format3) && (mode1.Width != parameters1.BackBufferHeight)) && ((mode1.Height != parameters1.BackBufferHeight) && (mode1.RefreshRate != parameters1.FullScreenRefreshRateInHz)))
                {
                    flag2 = false;
                    foreach (DisplayMode mode2 in adapter1.SupportedDisplayModes[format3])
                    {
                        if ((mode2.Width == parameters1.BackBufferWidth) && (mode2.Height == parameters1.BackBufferHeight))
                        {
                            flag3 = true;
                            if (mode2.RefreshRate == parameters1.FullScreenRefreshRateInHz)
                            {
                                flag2 = true;
                                break;
                            }
                        }
                    }
                }
                if (!flag2 && flag3)
                {
                    throw new ArgumentException("Resources.ValidateBackBufferDimsModeFullScreen");
                }
                if (!flag2)
                {
                    throw new ArgumentException("Resources.ValidateBackBufferHzModeFullScreen");
                }
            }
            if (parameters1.EnableAutoDepthStencil != flag1)
            {
                throw new ArgumentException("Resources.ValidateAutoDepthStencilAdapterGroup");
            }
            if (parameters1.EnableAutoDepthStencil)
            {
                if (parameters1.AutoDepthStencilFormat != format1)
                {
                    throw new ArgumentException("Resources.ValidateAutoDepthStencilAdapterGroup");
                }
                if (parameters1.BackBufferFormat != format2)
                {
                    throw new ArgumentException("Resources.ValidateAutoDepthStencilAdapterGroup");
                }
                if (parameters1.BackBufferWidth != num1)
                {
                    throw new ArgumentException("Resources.ValidateAutoDepthStencilAdapterGroup");
                }
                if (parameters1.BackBufferHeight != num2)
                {
                    throw new ArgumentException("Resources.ValidateAutoDepthStencilAdapterGroup");
                }
            }
        }

        // Properties
        public GraphicsDevice GraphicsDevice
        {
            get
            {
                return this.device;
            }
        }
        public bool IsFullScreen
        {
            get
            {
                return this.isFullScreen;
            }
            set
            {
                this.isFullScreen = value;
                this.isDeviceDirty = true;
            }
        }
        public ShaderProfile MinimumPixelShaderProfile
        {
            get
            {
                return this.minimumPixelShaderProfile;
            }
            set
            {
                if ((value < ShaderProfile.PS_1_1) || (value > ShaderProfile.XPS_3_0))
                {
                    throw new ArgumentOutOfRangeException("value", "Resources.InvalidPixelShaderProfile");
                }
                this.minimumPixelShaderProfile = value;
                this.isDeviceDirty = true;




            }
        }
        public ShaderProfile MinimumVertexShaderProfile
        {
            get
            {
                return this.minimumVertexShaderProfile;
            }
            set
            {
                if ((value < ShaderProfile.VS_1_1) || (value > ShaderProfile.XVS_3_0))
                {
                    throw new ArgumentOutOfRangeException("value", "Resources.InvalidVertexShaderProfile");
                }
                this.minimumVertexShaderProfile = value;
                this.isDeviceDirty = true;
            }
        }
        public bool PreferMultiSampling
        {
            get
            {
                return this.allowMultiSampling;
            }
            set
            {
                this.allowMultiSampling = value;
                this.isDeviceDirty = true;
            }
        }
        public SurfaceFormat PreferredBackBufferFormat
        {
            get
            {
                return this.backBufferFormat;
            }
            set
            {
                if (Array.IndexOf<SurfaceFormat>(GraphicsDeviceManager.ValidBackBufferFormats, value) == -1)
                {
                    throw new ArgumentOutOfRangeException("value", "Resources.ValidateBackBufferFormatIsInvalid");
                }
                this.backBufferFormat = value;
                this.isDeviceDirty = true;
            }
        }
        public int PreferredBackBufferHeight
        {
            get
            {
                return this.backBufferHeight;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value", "Resources.BackBufferDimMustBePositive");
                }
                this.backBufferHeight = value;
                this.useResizedBackBuffer = false;
                this.isDeviceDirty = true;
            }
        }
        public int PreferredBackBufferWidth
        {
            get
            {
                return this.backBufferWidth;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value", "Resources.BackBufferDimMustBePositive");
                }
                this.backBufferWidth = value;
                this.useResizedBackBuffer = false;
                this.isDeviceDirty = true;
            }
        }
        public DepthFormat PreferredDepthStencilFormat
        {
            get
            {
                return this.depthStencilFormat;
            }
            set
            {
                switch (value)
                {
                    case DepthFormat.Depth24Stencil8:
                    case DepthFormat.Depth24Stencil8Single:
                    case DepthFormat.Depth24Stencil4:
                    case DepthFormat.Depth24:
                    case DepthFormat.Depth32:
                    case DepthFormat.Depth16:
                    case DepthFormat.Depth15Stencil1:
                        this.depthStencilFormat = value;
                        this.isDeviceDirty = true;
                        return;
                }
                throw new ArgumentOutOfRangeException("value", "Resources.ValidateDepthStencilFormatIsInvalid");
            }
        }
        public bool SynchronizeWithVerticalRetrace
        {
            get
            {
                return this.synchronizeWithVerticalRetrace;
            }
            set
            {
                this.synchronizeWithVerticalRetrace = value;
                this.isDeviceDirty = true;
            }
        }

        private static readonly TimeSpan deviceLostSleepTime = TimeSpan.FromMilliseconds(50);
        private static MultiSampleType[] multiSampleTypes = new MultiSampleType[] {
 MultiSampleType.NonMaskable,
 MultiSampleType.SixteenSamples,
 MultiSampleType.FifteenSamples,
 MultiSampleType.FourteenSamples,
 MultiSampleType.ThirteenSamples,
 MultiSampleType.TwelveSamples,
 MultiSampleType.ElevenSamples,
 MultiSampleType.TenSamples,
 MultiSampleType.NineSamples,
 MultiSampleType.EightSamples,
 MultiSampleType.SevenSamples,
 MultiSampleType.SixSamples,
 MultiSampleType.FiveSamples,
 MultiSampleType.FourSamples,
 MultiSampleType.ThreeSamples,
 MultiSampleType.TwoSamples
 };

        // Fields
        private bool allowMultiSampling;
        private SurfaceFormat backBufferFormat;
        private int backBufferHeight;
        private int backBufferWidth;
        private bool beginDrawOk;
        private DepthFormat depthStencilFormat;
        private GraphicsDevice device;
        private EventHandler deviceCreated;
        private EventHandler deviceDisposing;
        private EventHandler deviceReset;
        private EventHandler deviceResetting;
        //private EventHandler Disposed;
        private Game game;
        private bool inDeviceTransition;
        private bool isDeviceDirty;
        private bool isFullScreen;
        private bool isReallyFullScreen;
        private ShaderProfile minimumPixelShaderProfile;
        private ShaderProfile minimumVertexShaderProfile;
        //private EventHandler<PreparingDeviceSettingsEventArgs> PreparingDeviceSettings;
        private int resizedBackBufferHeight;
        private int resizedBackBufferWidth;
        private bool synchronizeWithVerticalRetrace;
        private bool useResizedBackBuffer;

        #region WindowsGame
        internal static Screen ScreenFromAdapter(GraphicsAdapter adapter)
        {
            foreach (Screen screen1 in Screen.AllScreens)
            {
                if (DeviceNameFromScreen(screen1) == adapter.DeviceName)
                {
                    return screen1;
                }
            }
            throw new ArgumentException("Resources.InvalidScreenAdapter", "adapter");
        }
        internal static string DeviceNameFromScreen(Screen screen)
        {
            string text1 = screen.DeviceName;
            int num1 = screen.DeviceName.IndexOf('\0');
            if (num1 != -1)
            {
                text1 = screen.DeviceName.Substring(0, num1);
            }
            return text1;
        }
        internal static Screen ScreenFromHandle(IntPtr windowHandle)
        {
            RECT rect1;
            int num1 = 0;
            Screen screen1 = null;
            GetWindowRect(windowHandle, out rect1);
            System.Drawing.Rectangle rectangle1 = new System.Drawing.Rectangle(
            rect1.Left, rect1.Top, rect1.Right - rect1.Left, rect1.Bottom - rect1.Top
            );
            foreach (Screen screen2 in Screen.AllScreens)
            {
                System.Drawing.Rectangle rectangle2 = rectangle1;
                rectangle2.Intersect(screen2.Bounds);
                int num2 = rectangle2.Width * rectangle2.Height;
                if (num2 > num1)
                {
                    num1 = num2;
                    screen1 = screen2;
                }
            }
            if (screen1 == null)
            {
                screen1 = Screen.AllScreens[0];
            }
            return screen1;
        }
        #endregion
        #region NativeMethods
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressUnmanagedCodeSecurity, DllImport("user32.dll")]
        internal static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressUnmanagedCodeSecurity, DllImport("user32.dll")]
        internal static extern bool GetClientRect(IntPtr hWnd, out RECT rect);
        #endregion
        #region GraphicsDeviceInformationComparer
        internal class GraphicsDeviceInformationComparer : IComparer<GraphicsDeviceInformation>
        {
            // Methods
            public GraphicsDeviceInformationComparer(ReferenceGraphicsDeviceManager graphicsComponent)
            {
                this.graphics = graphicsComponent;
            }
            public int Compare(GraphicsDeviceInformation d1, GraphicsDeviceInformation d2)
            {
                float single1;
                if (d1.DeviceType != d2.DeviceType)
                {
                    if (d1.DeviceType >= d2.DeviceType)
                    {
                        return 1;
                    }
                    return -1;
                }
                //CreateOptions options1 = CreateOptions.MixedVertexProcessing | CreateOptions.HardwareVertexProcessing | CreateOptions.SoftwareVertexProcessing;




                //CreateOptions options2 = d1.CreationOptions & options1;
                //CreateOptions options3 = d2.CreationOptions & options1;
                //if (options2 != options3) {
                // if (options2 == CreateOptions.HardwareVertexProcessing) {
                // return -1;
                // }
                // if (options3 != CreateOptions.HardwareVertexProcessing) {
                // if (options2 == CreateOptions.MixedVertexProcessing) {
                // return -1;
                // }
                // if (options3 == CreateOptions.MixedVertexProcessing) {
                // return 1;
                // }
                // if (options2 != CreateOptions.None) {
                // return -1;
                // }
                // }
                // return 1;
                //}
                PresentationParameters parameters1 = d1.PresentationParameters;
                PresentationParameters parameters2 = d2.PresentationParameters;
                if (parameters1.IsFullScreen != parameters2.IsFullScreen)
                {
                    if (this.graphics.IsFullScreen != parameters1.IsFullScreen)
                    {
                        return 1;
                    }
                    return -1;
                }
                int num1 = this.RankFormat(parameters1.BackBufferFormat);
                int num2 = this.RankFormat(parameters2.BackBufferFormat);
                if (num1 != num2)
                {
                    if (num1 >= num2)
                    {
                        return 1;
                    }
                    return -1;
                }
                if (parameters1.MultiSampleType != parameters2.MultiSampleType)
                {
                    int num3 = (parameters1.MultiSampleType == MultiSampleType.NonMaskable) ? ((int)(MultiSampleType.SixteenSamples | MultiSampleType.NonMaskable)) : ((int)parameters1.MultiSampleType);
                    int num4 = (parameters2.MultiSampleType == MultiSampleType.NonMaskable) ? ((int)(MultiSampleType.SixteenSamples | MultiSampleType.NonMaskable)) : ((int)parameters2.MultiSampleType);
                    if (num3 <= num4)
                    {
                        return 1;
                    }
                    return -1;
                }
                if (parameters1.MultiSampleQuality != parameters2.MultiSampleQuality)
                {
                    if (parameters1.MultiSampleQuality <= parameters2.MultiSampleQuality)
                    {
                        return 1;
                    }
                    return -1;
                }
                if ((this.graphics.PreferredBackBufferWidth == 0) || (this.graphics.PreferredBackBufferHeight == 0))
                {
                    single1 = ((float)GraphicsDeviceManager.DefaultBackBufferWidth) / ((float)GraphicsDeviceManager.DefaultBackBufferHeight);
                }
                else
                {
                    single1 = ((float)this.graphics.PreferredBackBufferWidth) / ((float)this.graphics.PreferredBackBufferHeight);
                }
                float single2 = ((float)parameters1.BackBufferWidth) / ((float)parameters1.BackBufferHeight);
                float single3 = ((float)parameters2.BackBufferWidth) / ((float)parameters2.BackBufferHeight);
                float single4 = Math.Abs((float)(single2 - single1));
                float single5 = Math.Abs((float)(single3 - single1));
                if (Math.Abs((float)(single4 - single5)) > 0.2f)
                {
                    if (single4 >= single5)
                    {
                        return 1;
                    }
                    return -1;
                }
                int num5 = 0;
                int num6 = 0;
                if (this.graphics.IsFullScreen)
                {
                    if ((this.graphics.PreferredBackBufferWidth == 0) || (this.graphics.PreferredBackBufferHeight == 0))
                    {
                        GraphicsAdapter adapter1 = d1.Adapter;
                        num5 = adapter1.CurrentDisplayMode.Width * adapter1.CurrentDisplayMode.Height;
                        GraphicsAdapter adapter2 = d2.Adapter;
                        num6 = adapter2.CurrentDisplayMode.Width * adapter2.CurrentDisplayMode.Height;
                    }
                    else
                    {
                        num5 = num6 = this.graphics.PreferredBackBufferWidth * this.graphics.PreferredBackBufferHeight;
                    }
                }
                else if ((this.graphics.PreferredBackBufferWidth == 0) || (this.graphics.PreferredBackBufferHeight == 0))
                {
                    num5 = num6 = GraphicsDeviceManager.DefaultBackBufferWidth * GraphicsDeviceManager.DefaultBackBufferHeight;
                }
                else
                {
                    num5 = num6 = this.graphics.PreferredBackBufferWidth * this.graphics.PreferredBackBufferHeight;
                }
                int num7 = Math.Abs((int)((parameters1.BackBufferWidth * parameters1.BackBufferHeight) - num5));
                int num8 = Math.Abs((int)((parameters2.BackBufferWidth * parameters2.BackBufferHeight) - num6));
                if (num7 != num8)
                {
                    if (num7 >= num8)
                    {
                        return 1;
                    }
                    return -1;
                }
                if (this.graphics.IsFullScreen && (parameters1.FullScreenRefreshRateInHz != parameters2.FullScreenRefreshRateInHz))
                {
                    if (parameters1.FullScreenRefreshRateInHz <= parameters2.FullScreenRefreshRateInHz)
                    {
                        return 1;
                    }
                    return -1;
                }
                if (d1.Adapter != d2.Adapter)
                {
                    if (d1.Adapter.IsDefaultAdapter)
                    {
                        return -1;
                    }
                    if (d2.Adapter.IsDefaultAdapter)
                    {
                        return 1;
                    }
                }
                return 0;
            }
            private int RankFormat(SurfaceFormat format)
            {
                int num1 = Array.IndexOf<SurfaceFormat>(GraphicsDeviceManager.ValidBackBufferFormats, format);
                if (num1 != -1)
                {
                    int num2 = Array.IndexOf<SurfaceFormat>(GraphicsDeviceManager.ValidBackBufferFormats, this.graphics.PreferredBackBufferFormat);
                    if (num2 == -1)
                    {
                        return (GraphicsDeviceManager.ValidBackBufferFormats.Length - num1);
                    }
                    if (num1 >= num2)
                    {
                        return (num1 - num2);
                    }
                }
                return 0x7fffffff;
            }

            // Fields
            private ReferenceGraphicsDeviceManager graphics;
        }
        #endregion
    }
}


