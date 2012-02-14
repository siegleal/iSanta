//
//  CSVEmailTesterViewController.m
//  CSVEmailTester
//
//  Created by Eric Henderson on 2/8/12.
//

#import "CSVEmailTesterViewController.h"

@implementation CSVEmailTesterViewController

- (IBAction)sendEmailClicked:(id)sender
{
    if ([MFMailComposeViewController canSendMail])
    {
        MFMailComposeViewController *mailViewController = [[MFMailComposeViewController alloc] init];
        mailViewController.mailComposeDelegate = self;
        [mailViewController setSubject:@"Analysis Results"];
        NSString *adata = @"General Info\nShooter, Eric\nDate Fired, 2/3/2012\nPlace, RHIT\nTemperature, 24 C\nTarget Distance, 100 yards\nShots Fired, 5\n\nSerial #, 12345\nProjectile, 50 cal\nLot #, 25\nProjectile Mass, 5g\n\n\nStatistics (in inches)\nExtreme Spread X, 10\nExtreme Spread Y, 10\nExtreme Spread Group, 10\nMean Radius, 5\nSigma X, 0.5\nSigma Y, 0.5\nFurthest Left, -5\nFurthest Right, 5\nHighest Round, 5\nLowest Round, -5\nCEP Radius, 5\n\n\nShot Record (in inches)\nPoint #, X Value, Y Value\n1, -5, 0\n2, 0, 5\n3, 0, 0\n4, 0, -5\n5, 5, 0";
        [mailViewController addAttachmentData:[adata dataUsingEncoding:NSASCIIStringEncoding] mimeType:@"text/csv" fileName:@"results.csv"];
        [self presentModalViewController:mailViewController animated:YES];
    }
    else
    {
        NSLog(@"Device is unable to send email in its current state.");
    }
}

- (void)mailComposeController:(MFMailComposeViewController *)controller
          didFinishWithResult:(MFMailComposeResult)result
                        error:(NSError *)error
{
    [self dismissModalViewControllerAnimated:YES];
}

@end
