namespace Captura.Video
{
    // ReSharper disable once ClassNeverInstantiated.Global
    class FullScreenItem : NotifyPropertyChanged, IVideoItem//, IVideoItem
    {
        readonly IPlatformServices _platformServices;
        /*readonly VideoSettings _videoSettings;*/

        public FullScreenItem(IPlatformServices PlatformServices/*, VideoSettings VideoSettings*/)
        {
            _platformServices = PlatformServices;
            /*_videoSettings = VideoSettings;*/
        }

        public override string ToString() => Name;

        public string Name => null;

        public IImageProvider GetImageProvider(bool IncludeCursor)
        {
            return _platformServices.GetAllScreensProvider(IncludeCursor, false);
		}
    }
}