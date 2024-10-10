using System.Text.RegularExpressions;

namespace VideoPlayerDiscordBot.Service
{
    public class ValidationService(IDownloadService downloadService) : IValidationService
    {
        IDownloadService _downloadService = downloadService;

        public string ValidateLink(string args)
        {
            
            if(!args.Contains("youtube.com") && !args.Contains("youtu.be"))
            {
                return $"{args} is not a valid link.";
            }
            Console.WriteLine(args);
            string pattern = @"=(.*)";

            Match match = Regex.Match(args, pattern);
            if (!match.Success)
            {
                return $"{args} is not a valid link.";
            }
            string fileName = match.Groups[1].Value;
            string filelocation = Path.Combine(Program.downloadPath, fileName);
            try
            {
                Directory.CreateDirectory(filelocation);
            } catch (Exception ex){
                Console.WriteLine(ex.Message);
                return ex.Message;
            }

            fileName = Path.Combine(filelocation, fileName);
            Task.Run(() => _downloadService.DownloadVideo(filelocation, args, fileName));
            return "Video added";
        }
    }
}