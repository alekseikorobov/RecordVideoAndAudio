using Captura.Video;
using Captura.Windows.MediaFoundation;

namespace Captura.FFmpeg
{
    abstract class FFmpegVideoCodec : IVideoWriterItem
    {
        protected FFmpegVideoCodec(string Name, string Extension, string Description)
        {
            this.Name = Name;
            this.Extension = Extension;
            this.Description = Description;
        }

        public virtual string Name { get; }

        public virtual string Extension { get; }

        public string Description { get; }

        public abstract FFmpegAudioArgsProvider AudioArgsProvider { get; }

        public abstract void Apply(FFmpegSettings Settings, VideoWriterArgs WriterArgs, FFmpegOutputArgs OutputArgs);

        public virtual IVideoFileWriter GetVideoFileWriter(VideoWriterArgs Args)
        {
            var _device = new SharpDX.Direct3D11.Device(SharpDX.Direct3D.DriverType.Hardware, SharpDX.Direct3D11.DeviceCreationFlags.BgraSupport);
            return new MfWriter(Args, _device);
        }

        public override string ToString() => Name;
    }
}
