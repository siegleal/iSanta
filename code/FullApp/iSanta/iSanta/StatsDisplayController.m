//
//  StatsDisplayController.m
//  iSanta
//
//  Created by Eric Henderson on 4/27/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "StatsDisplayController.h"
#import "PointsDisplayController.h"
#import "DefaultStatsProvider.h"

@implementation StatsDisplayController

@synthesize statsProvider = _statsProvider;
@synthesize points = _points;
@synthesize reportData = _reportData;
@synthesize targetPhoto = _targetPhoto;

- (IBAction)launchEmail:(id)sender
{
    if ([MFMailComposeViewController canSendMail])
    {
        MFMailComposeViewController *mailViewController = [[MFMailComposeViewController alloc] init];
        mailViewController.mailComposeDelegate = self;
        [mailViewController setSubject:@"Analysis Results"];
        NSString *adata = @"General Info,,\n";
        adata = [adata stringByAppendingFormat:@"%@,%@,\n", @"Shooter", [self.reportData objectForKey:@"Shooter"]];
        adata = [adata stringByAppendingFormat:@"%@,%@,\n", @"Date Fired", [self.reportData objectForKey:@"Date Fired"]];
        adata = [adata stringByAppendingFormat:@"%@,%@,\n", @"Place", [self.reportData objectForKey:@"Place"]];
        adata = [adata stringByAppendingFormat:@"%@,%@,\n", @"Temperature", [self.reportData objectForKey:@"Temperature"]];
        adata = [adata stringByAppendingFormat:@"%@,%@,\n", @"Target Distance", [self.reportData objectForKey:@"Target Distance"]];
        adata = [adata stringByAppendingFormat:@"%@,%@,\n", @"Shots Fired", [self.reportData objectForKey:@"Shots Fired"]];
        adata = [adata stringByAppendingFormat:@"%@,%@,\n", @"Weapon Nomenclature", [self.reportData objectForKey:@"Weapon Nomenclature"]];
        adata = [adata stringByAppendingFormat:@"%@,%@,\n", @"Weapon Serial #", [self.reportData objectForKey:@"Weapon Serial #"]];
        adata = [adata stringByAppendingFormat:@"%@,%@,\n", @"Projectile Caliber", [self.reportData objectForKey:@"Projectile Caliber"]];
        adata = [adata stringByAppendingFormat:@"%@,%@,\n", @"Lot #", [self.reportData objectForKey:@"Lot #"]];
        adata = [adata stringByAppendingFormat:@"%@,%@,\n", @"Projectile Mass", [self.reportData objectForKey:@"Projectile Mass"]];
        adata = [adata stringByAppendingString:@",,\nStatistics (in inches),,\n"];
        int count = [self.statsProvider getRowCount:self.points];
        
        //convert the points to measurement from pixels
        //get normalization points
        double pixelHeight = 1;
        double pixelWidth = 1;

        if (self.points.count >3){
            pixelHeight = fabs([[self.points objectAtIndex:0] doubleValue] - [[self.points objectAtIndex:1] doubleValue]);
            pixelWidth = fabs([[self.points objectAtIndex:1] doubleValue] - [[self.points objectAtIndex:2] doubleValue]);
            [self.points removeObjectAtIndex:0];
            [self.points removeObjectAtIndex:1];
            [self.points removeObjectAtIndex:2];
        }
        for(int i = 0; i < self.points.count; i++)
        {
            NSValue *v = [self.points objectAtIndex:i];
            CGPoint p = v.CGPointValue;
            //convert for pixels to actual length
            double xRatio = [[self.reportData objectForKey:@"Target Height"] doubleValue] / pixelHeight;
            double yRatio = [[self.reportData objectForKey:@"Target Width"] doubleValue] / pixelWidth;
            p.x *= xRatio;
            p.y *= yRatio;
            [self.points replaceObjectAtIndex:i withObject:[NSValue valueWithCGPoint:p]];
        }
        
        

        for(int i = 0; i < count; i++)
        {
            adata = [adata stringByAppendingFormat:@"%@,%@,\n", [self.statsProvider getTitleForIndex:i withPoints:self.points], [self.statsProvider getValueForIndex:i withPoints:self.points]];
        }
        adata = [adata stringByAppendingString:@",,\n,,\nShot Record (in inches),,\nPoint #,X Value, Y Value\n"];
                if (self.points.count >3){
            pixelHeight = fabs([[self.points objectAtIndex:0] doubleValue] - [[self.points objectAtIndex:1] doubleValue]);
            pixelWidth = fabs([[self.points objectAtIndex:1] doubleValue] - [[self.points objectAtIndex:2] doubleValue]);
        }
        

        for(int i = 0; i < self.points.count; i++)
        {
            NSValue *v = [self.points objectAtIndex:i];
            CGPoint p = v.CGPointValue;
            adata = [adata stringByAppendingFormat:@"%d,%.2f,%.2f\n",(i+1),p.x, p.y];
        }
        [mailViewController addAttachmentData:[adata dataUsingEncoding:NSASCIIStringEncoding] mimeType:@"text/csv" fileName:@"results.csv"];
        [mailViewController addAttachmentData:self.targetPhoto mimeType:@"image/jpg" fileName:@"target.jpg"];
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

- (id<StatsProvider>)statsProvider
{
    if(!_statsProvider)
        _statsProvider = [[DefaultStatsProvider alloc] init];
    return _statsProvider;
}

- (id)initWithStyle:(UITableViewStyle)style
{
    self = [super initWithStyle:style];
    if (self) {
        // Custom initialization
    }
    return self;
}

- (void)didReceiveMemoryWarning
{
    // Releases the view if it doesn't have a superview.
    [super didReceiveMemoryWarning];
    
    // Release any cached data, images, etc that aren't in use.
}

#pragma mark - View lifecycle

- (void)viewDidLoad
{
    [super viewDidLoad];

    // Uncomment the following line to preserve selection between presentations.
    // self.clearsSelectionOnViewWillAppear = NO;
 
    // Uncomment the following line to display an Edit button in the navigation bar for this view controller.
    // self.navigationItem.rightBarButtonItem = self.editButtonItem;
}

- (void)viewDidUnload
{
    [super viewDidUnload];
    // Release any retained subviews of the main view.
    // e.g. self.myOutlet = nil;
}

- (void)viewWillAppear:(BOOL)animated
{
    [super viewWillAppear:animated];
}

- (void)viewDidAppear:(BOOL)animated
{
    [super viewDidAppear:animated];
}

- (void)viewWillDisappear:(BOOL)animated
{
    [super viewWillDisappear:animated];
}

- (void)viewDidDisappear:(BOOL)animated
{
    [super viewDidDisappear:animated];
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
    // Return YES for supported orientations
    return (interfaceOrientation == UIInterfaceOrientationPortrait);
}

#pragma mark - Table view data source

- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView
{
    // Return the number of sections.
    return 3;
}

- (NSString *)tableView:(UITableView *)tableView titleForHeaderInSection:(NSInteger)section
{
    if(section == 0)
        return @"General Info";
    else if(section == 1)
        return @"Statistics (in inches)";
    else
        return @"Shot Record (in inches)";
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section
{
    // Return the number of rows in the section.
    if(section == 0)
        return 11;
    else if(section == 1)
        //return 11;
        return [self.statsProvider getRowCount:self.points];
    else
        return 1;
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath
{
    NSString *CellIdentifier = @"SubtitleCell";
    if(indexPath.section == 2)
        CellIdentifier = @"PointsCell";
    
    UITableViewCell *cell = [tableView dequeueReusableCellWithIdentifier:CellIdentifier];
    if (cell == nil) {
        cell = [[UITableViewCell alloc] initWithStyle:UITableViewCellStyleDefault reuseIdentifier:CellIdentifier];
    }
    
    if(indexPath.section == 0)
    {
        if(indexPath.row == 0)
        {
            cell.textLabel.text = @"Shooter";
            //cell.detailTextLabel.text = @"Eric";
            cell.detailTextLabel.text = [self.reportData objectForKey:@"Shooter"];
        }
        else if(indexPath.row == 1)
        {
            cell.textLabel.text = @"Date Fired";
            //cell.detailTextLabel.text = @"2/3/2012";
            cell.detailTextLabel.text = [self.reportData objectForKey:@"Date Fired"];
        }
        else if(indexPath.row == 2)
        {
            cell.textLabel.text = @"Place";
            //cell.detailTextLabel.text = @"RHIT";
            cell.detailTextLabel.text = [self.reportData objectForKey:@"Place"];
        }
        else if(indexPath.row == 3)
        {
            cell.textLabel.text = @"Temperature";
            //cell.detailTextLabel.text = @"24 C";
            cell.detailTextLabel.text = [self.reportData objectForKey:@"Temperature"];
        }
        else if(indexPath.row == 4)
        {
            cell.textLabel.text = @"Target Distance";
            //cell.detailTextLabel.text = @"100 yards";
            cell.detailTextLabel.text = [self.reportData objectForKey:@"Target Distance"];
        }
        else if(indexPath.row == 5)
        {
            cell.textLabel.text = @"Shots Fired";
            //cell.detailTextLabel.text = @"5";
            cell.detailTextLabel.text = [self.reportData objectForKey:@"Shots Fired"];
        }
        else if(indexPath.row == 6)
        {
            cell.textLabel.text = @"Weapon Nomenclature";
            //cell.detailTextLabel.text = @"12345";
            cell.detailTextLabel.text = [self.reportData objectForKey:@"Weapon Nomenclature"];
        }
        else if(indexPath.row == 7)
        {
            cell.textLabel.text = @"Weapon Serial #";
            //cell.detailTextLabel.text = @"12345";
            cell.detailTextLabel.text = [self.reportData objectForKey:@"Weapon Serial #"];
        }
        else if(indexPath.row == 8)
        {
            cell.textLabel.text = @"Projectile Caliber";
            //cell.detailTextLabel.text = @"50 cal";
            cell.detailTextLabel.text = [self.reportData objectForKey:@"Projectile Caliber"];
        }
        else if(indexPath.row == 9)
        {
            cell.textLabel.text = @"Lot #";
            //cell.detailTextLabel.text = @"25";
            cell.detailTextLabel.text = [self.reportData objectForKey:@"Lot #"];
        }
        else if(indexPath.row == 10)
        {
            cell.textLabel.text = @"Projectile Mass";
            //cell.detailTextLabel.text = @"5g";
            cell.detailTextLabel.text = [self.reportData objectForKey:@"Projectile Mass"];
        }
    }
    else if(indexPath.section == 1)
    {
        cell.textLabel.text = [self.statsProvider getTitleForIndex:indexPath.row withPoints:self.points];
        cell.detailTextLabel.text = [self.statsProvider getValueForIndex:indexPath.row withPoints:self.points];
        /*if(indexPath.row == 0)
        {
            cell.textLabel.text = @"Extreme Spread X";
            cell.detailTextLabel.text = @"10";
        }
        else if(indexPath.row == 1)
        {
            cell.textLabel.text = @"Extreme Spread Y";
            cell.detailTextLabel.text = @"10";
        }
        else if(indexPath.row == 2)
        {
            cell.textLabel.text = @"Extreme Spread Group";
            cell.detailTextLabel.text = @"10";
        }
        else if(indexPath.row == 3)
        {
            cell.textLabel.text = @"Mean Radius";
            cell.detailTextLabel.text = @"5";
        }
        else if(indexPath.row == 4)
        {
            cell.textLabel.text = @"Sigma X";
            cell.detailTextLabel.text = @"0.5";
        }
        else if(indexPath.row == 5)
        {
            cell.textLabel.text = @"Sigma Y";
            cell.detailTextLabel.text = @"0.5";
        }
        else if(indexPath.row == 6)
        {
            cell.textLabel.text = @"Furthest Left";
            cell.detailTextLabel.text = @"-5";
        }
        else if(indexPath.row == 7)
        {
            cell.textLabel.text = @"Furthest Right";
            cell.detailTextLabel.text = @"5";
        }
        else if(indexPath.row == 8)
        {
            cell.textLabel.text = @"Highest Round";
            cell.detailTextLabel.text = @"5";
        }
        else if(indexPath.row == 9)
        {
            cell.textLabel.text = @"Lowest Round";
            cell.detailTextLabel.text = @"-5";
        }
        else if(indexPath.row == 10)
        {
            cell.textLabel.text = @"CEP Radius";
            cell.detailTextLabel.text = @"5";
        }*/
    }
    else
        cell.textLabel.text = @"Points";
    
    return cell;
}

/*
// Override to support conditional editing of the table view.
- (BOOL)tableView:(UITableView *)tableView canEditRowAtIndexPath:(NSIndexPath *)indexPath
{
    // Return NO if you do not want the specified item to be editable.
    return YES;
}
*/

/*
// Override to support editing the table view.
- (void)tableView:(UITableView *)tableView commitEditingStyle:(UITableViewCellEditingStyle)editingStyle forRowAtIndexPath:(NSIndexPath *)indexPath
{
    if (editingStyle == UITableViewCellEditingStyleDelete) {
        // Delete the row from the data source
        [tableView deleteRowsAtIndexPaths:[NSArray arrayWithObject:indexPath] withRowAnimation:UITableViewRowAnimationFade];
    }   
    else if (editingStyle == UITableViewCellEditingStyleInsert) {
        // Create a new instance of the appropriate class, insert it into the array, and add a new row to the table view
    }   
}
*/

/*
// Override to support rearranging the table view.
- (void)tableView:(UITableView *)tableView moveRowAtIndexPath:(NSIndexPath *)fromIndexPath toIndexPath:(NSIndexPath *)toIndexPath
{
}
*/

/*
// Override to support conditional rearranging of the table view.
- (BOOL)tableView:(UITableView *)tableView canMoveRowAtIndexPath:(NSIndexPath *)indexPath
{
    // Return NO if you do not want the item to be re-orderable.
    return YES;
}
*/

#pragma mark - Table view delegate

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath
{
    // Navigation logic may go here. Create and push another view controller.
    /*
     <#DetailViewController#> *detailViewController = [[<#DetailViewController#> alloc] initWithNibName:@"<#Nib name#>" bundle:nil];
     // ...
     // Pass the selected object to the new view controller.
     [self.navigationController pushViewController:detailViewController animated:YES];
     */
}

- (void)prepareForSegue:(UIStoryboardSegue *)segue sender:(id)sender
{
    if ([[segue identifier] isEqualToString:@"StatsToPoints"])
    {
        //NSIndexPath *selectedRowIndex = [self.tableView indexPathForSelectedRow];
        PointsDisplayController *cont = [segue destinationViewController];
        cont.points = self.points;
    }
}

@end
