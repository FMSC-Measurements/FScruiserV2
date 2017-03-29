using System;

using System.Collections.Generic;
using System.Text;
using FScruiser.Core.Services;
using System.Windows.Forms;
using FSCruiser.Core;

#if NetCF
using SoundPlayer = OpenNETCF.Media.SoundPlayer;
using System.IO;
#else

using SoundPlayer = System.Media.SoundPlayer;
using System.IO;

#endif

namespace FSCruiser.WinForms
{
    public class WinFormsSoundService : ISoundService
    {
        SoundPlayer _tallySoundPlayer;
        SoundPlayer _pageChangedSoundPlayer;
        SoundPlayer _measureSoundPlayer;
        SoundPlayer _insuranceSoundPlayer;

        public WinFormsSoundService()
        {
            try
            {
                var soundsDir = System.IO.Path.Combine(GetExecutionDirectory(), "Sounds");

                _tallySoundPlayer = new SoundPlayer(new FileStream(soundsDir + "\\tally.wav", System.IO.FileMode.Open));
                _pageChangedSoundPlayer = new SoundPlayer(new FileStream(soundsDir + "\\pageChange.wav", FileMode.Open));
                _measureSoundPlayer = new SoundPlayer(new FileStream(soundsDir + "\\measure.wav", FileMode.Open));
                _insuranceSoundPlayer = new SoundPlayer(new FileStream(soundsDir + "\\insurance.wav", FileMode.Open));
            }
            catch
            {
            }
        }

        static string GetExecutionDirectory()
        {
            string name = System.Reflection.Assembly
                .GetCallingAssembly()
                .GetName().CodeBase;

            //clean up path, in FF name is a URI
            if (name.StartsWith(@"file:///"))
            {
                name = name.Replace("file:///", string.Empty);
            }
            string dir = System.IO.Path.GetDirectoryName(name);
            return dir;
        }

        public void SignalInvalidAction()
        {
#if NetCF
            FSCruiser.WinForms.Win32.MessageBeep(-1);
#else
            System.Media.SystemSounds.Beep.Play();
#endif
        }

        public void SignalMeasureTree()
        {
            if (_measureSoundPlayer != null)
            {
                _measureSoundPlayer.Play();
            }
            else
            {
#if NetCF
                //Win32.MessageBeep(Win32.MB_ICONQUESTION);
#else
                System.Media.SystemSounds.Exclamation.Play();
#endif
            }
        }

        public void SignalInsuranceTree()
        {
            if (_insuranceSoundPlayer != null)
            {
                _insuranceSoundPlayer.Play();
            }
            else
            {
#if NetCF
                Win32.MessageBeep(Win32.MB_ICONASTERISK);
#else
                System.Media.SystemSounds.Asterisk.Play();
#endif
            }
        }

        public void SignalTally(bool force)
        {
            var settings = ApplicationSettings.Instance;
            if (settings.EnableTallySound
                || force)
            {
                if (_tallySoundPlayer != null)
                {
                    _tallySoundPlayer.Play();
                }
            }
        }

        public void SignalPageChanged(bool force)
        {
            var settings = ApplicationSettings.Instance;
            if (settings.EnablePageChangeSound
                || force)
            {
                if (_pageChangedSoundPlayer != null)
                {
                    _pageChangedSoundPlayer.Play();
                }
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
#if NetCF
                if (_pageChangedSoundPlayer != null)
                {
                    _pageChangedSoundPlayer.Dispose();
                    _pageChangedSoundPlayer = null;
                }
                if (_tallySoundPlayer != null)
                {
                    _tallySoundPlayer.Dispose();
                    _tallySoundPlayer = null;
                }
#endif
            }
        }

        #endregion IDisposable Members
    }
}