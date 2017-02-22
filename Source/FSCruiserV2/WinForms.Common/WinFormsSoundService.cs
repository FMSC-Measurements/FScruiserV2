using System;

using System.Collections.Generic;
using System.Text;
using FScruiser.Core.Services;
using System.Windows.Forms;
using FSCruiser.Core;

#if NetCF
using OpenNETCF.Media;
using System.IO;
#endif

namespace FSCruiser.WinForms
{
    public class WinFormsSoundService: ISoundService
    {
#if NetCF
        SoundPlayer _tallySoundPlayer;
        SoundPlayer _pageChangedSoundPlayer;
#endif 

        public WinFormsSoundService()
        {
#if NetCF
            try
            {
                var soundsDir = System.IO.Path.Combine(GetExecutionDirectory(), "Sounds");

                _tallySoundPlayer = new SoundPlayer(new FileStream(soundsDir + "\\tally.wav", System.IO.FileMode.Open));
                _pageChangedSoundPlayer = new SoundPlayer(new FileStream(soundsDir + "\\pageChange.wav", FileMode.Open));
            }
            catch
            {
            }
#endif
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

        public void SignalMeasureTree(bool showMessage)
        {
#if NetCF
            Win32.MessageBeep(Win32.MB_ICONQUESTION);
            if (showMessage)
            {
                MessageBox.Show("Measure Tree");
            }
#else
            System.Media.SystemSounds.Exclamation.Play();
            if (showMessage)
            {
                MessageBox.Show("Measure Tree");
            }
#endif
        }

        public void SignalInsuranceTree()
        {
#if NetCF
            //Win32.MessageBeep(Win32.MB_ICONASTERISK);
            MessageBox.Show("Insurance Tree");
#else
            System.Media.SystemSounds.Asterisk.Play();
            MessageBox.Show("Insurance Tree");
#endif
        }

        public void SignalTally()
        {
#if NetCF
            var settings = ApplicationSettings.Instance;
            if (settings.EnableTallySound
                && _tallySoundPlayer != null)
            {
                _tallySoundPlayer.Play();
            }
#else
            //not implemented
#endif
        }

        public void SignalPageChanged()
        {
            #if NetCF
            var settings = ApplicationSettings.Instance;
            if (settings.EnablePageChangeSound
                && _pageChangedSoundPlayer != null)
            {
                _pageChangedSoundPlayer.Play();
            }
#else
            //not implemented
#endif
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

        #endregion
    }
}
