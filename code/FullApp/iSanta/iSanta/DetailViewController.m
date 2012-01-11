//
//  DetailViewController.m
//  iSanta
//
//  Created by Jack Hall on 12/14/11.
//  Copyright (c) 2011 __MyCompanyName__. All rights reserved.
//

#import "DetailViewController.h"

@interface DetailViewController ()
@property (strong, nonatomic) UIPopoverController *masterPopoverController;
- (void)configureView;
@end

@implementation DetailViewController

@synthesize detailItem = _detailItem;
@synthesize masterPopoverController = _masterPopoverController;
@synthesize detailDescriptionTable = _detailDescriptionTable;
@synthesize targetImage;
@synthesize selectedIndexPath;

#pragma mark - Managing the detail item

- (void)setDetailItem:(id)newDetailItem
{
    if (_detailItem != newDetailItem) {
        _detailItem = newDetailItem;
        
        // Update the view.
        [self configureView];
    }

    if (self.masterPopoverController != nil) {
        [self.masterPopoverController dismissPopoverAnimated:YES];
    }        
}

- (void)configureView
{
    // Update the user interface for the detail item.

    if (self.detailItem) {
        [self.detailDescriptionTable reloadData];
    }
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Release any cached data, images, etc that aren't in use.
}

#pragma mark - View lifecycle

- (void)viewDidLoad
{
    [super viewDidLoad];
	// Do any additional setup after loading the view, typically from a nib.
    [self configureView];
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
    if ([[UIDevice currentDevice] userInterfaceIdiom] == UIUserInterfaceIdiomPhone) {
        return (interfaceOrientation != UIInterfaceOrientationPortraitUpsideDown);
    } else {
        return YES;
    }
}

#pragma mark - Table View Data Source

- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView
{
    NSInteger count = 5;
    return count;
}

- (BOOL)tableView:(UITableView *)tableView canEditRowAtIndexPath:(NSIndexPath *)indexPath
{
    return NO;
}

- (BOOL)tableView:(UITableView *)tableView canMoveRowAtIndexPath:(NSIndexPath *)indexPath
{
    return NO;
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath
{
    static NSString *CellIdentifier = @"Cell";
    
    UITableViewCell *cell = [tableView dequeueReusableCellWithIdentifier:CellIdentifier];
    
    if (cell == nil) {
        cell = [[UITableViewCell alloc] initWithStyle:UITableViewCellStyleValue1 reuseIdentifier:CellIdentifier];
    }
    
    switch (indexPath.section) {
        case 0:
            [cell.imageView setImage:[[UIImage alloc] initWithData:
                                    [self.detailItem valueForKeyPath:@"test_Photo.image"]]];
            [cell.textLabel setText:@"Photo"];
            [cell.detailTextLabel setText:@""];
            break;
        case 1:
            switch (indexPath.row) {
                case 0:
                    [cell.textLabel setText:@"First Name"];
                    [cell.detailTextLabel setText:[[self.detailItem 
                                                   valueForKeyPath:@"test_Shooter.first_Name"]
                                                   description]];
                    break;
                case 1:
                    [cell.textLabel setText:@"Last Name"];
                    [cell.detailTextLabel setText:[[self.detailItem 
                                                   valueForKeyPath:@"test_Shooter.last_Name"]
                                                   description]];
                    break;
                default:
                    break;
            }
            break;
        case 2:
            switch (indexPath.row) {
                case 0:
                    [cell.textLabel setText:@"Firing Range"];
                    [cell.detailTextLabel setText:[[self.detailItem 
                                                   valueForKeyPath:@"test_Range.firing_Range"]
                                                   description]];
                    break;
                case 1:
                    [cell.textLabel setText:@"Distance to Target"];
                    [cell.detailTextLabel setText:[[self.detailItem 
                                                   valueForKeyPath:@"test_Range.distance_To_Target"]
                                                   description]];
                    break;
                case 2:
                    [cell.textLabel setText:@"Range Temperature"];
                    [cell.detailTextLabel setText:[[self.detailItem valueForKeyPath:
                                                   @"test_Range.range_Temperature"]
                                                   description]];
                    break;
                default:
                    break;
            }
            break;
        case 3:
            switch (indexPath.row) {
                case 0:
                    [cell.textLabel setText:@"Serial Number"];
                    [cell.detailTextLabel setText:[[self.detailItem 
                                                   valueForKeyPath:@"test_Weapon.serial_Number"]
                                                   description]];
                    break;
                case 1:
                    [cell.textLabel setText:@"Weapon Nomenclature"];
                    [cell.detailTextLabel setText:[[self.detailItem valueForKeyPath:
                                                   @"test_Weapon.weapon_Nomenclature"]
                                                   description]];
                    break;
                case 2:
                    [cell.textLabel setText:@"Notes"];
                    [cell.detailTextLabel setText:[[self.detailItem 
                                                   valueForKeyPath:@"test_Weapon.weapon_Notes"]
                                                   description]];
                    break;
                default:
                    break;
            }
            break;
        case 4:
            switch (indexPath.row) {
                case 0:
                    [cell.textLabel setText:@"Caliber"];
                    [cell.detailTextLabel setText:[[self.detailItem 
                                                   valueForKeyPath:@"test_Ammunition.caliber"]
                                                   description]];
                    break;
                case 1:
                    [cell.textLabel setText:@"Lot Number"];
                    [cell.detailTextLabel setText:[[self.detailItem 
                                                   valueForKeyPath:@"test_Ammunition.lot_Number"]
                                                   description]];
                    break;
                case 2:
                    [cell.textLabel setText:@"Number of Shots Fired"];
                    [cell.detailTextLabel setText:[[self.detailItem valueForKeyPath:
                                                   @"test_Ammunition.number_Of_Shots"]
                                                   description]];
                    break;
                case 3:
                    [cell.textLabel setText:@"Projectile Mass"];
                    [cell.detailTextLabel setText:[[self.detailItem valueForKeyPath:
                                                   @"test_Ammunition.projectile_Mass"]
                                                   description]];
                    break;
                case 4:
                    [cell.textLabel setText:@"Notes"];
                    [cell.detailTextLabel setText:[[self.detailItem valueForKeyPath:
                                                   @"test_Ammunition.ammunition_Notes"]
                                                   description]];
                    break;
                default:
                    break;
            }
            break;
        default:
            break;
    }
    return cell;
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section
{
    switch (section) {
        case 0:
            return 1;
            break;
        case 1:
            return 2;
            break;
        case 2:
            return 3;
            break;
        case 3:
            return 3;
            break;
        case 4:
            return 5;
            break;
        default:
            return 0;
            break;
    }
}

- (NSString *)tableView:(UITableView *)tableView titleForHeaderInSection:(NSInteger)section
{
    switch (section) {
        case 0:
            return @"Target Photo";
            break;
        case 1:
            return @"Shooter Information";
            break;
        case 2:
            return @"Range Information";
            break;
        case 3:
            return @"Weapon Information";
            break;
        case 4:
            return @"Ammunition Information";
            break;
        default:
            return nil;
            break;
    }

}

#pragma mark Table View Delegate

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath
{
    selectedIndexPath = indexPath;
    
    if(indexPath.section == 0)
    {
        UIImagePickerController *pickerController = [[UIImagePickerController alloc] init];
        pickerController.delegate = self;
        pickerController.sourceType = UIImagePickerControllerSourceTypeCamera;
        [self presentModalViewController:pickerController animated:YES];
    }
    else if(indexPath.section == 2 && indexPath.row == 2)
    {
        UIActionSheet *temperatureSheet = [[UIActionSheet alloc] initWithTitle:@"Choose a Temperature" delegate:self cancelButtonTitle:@"Cancel" destructiveButtonTitle:nil otherButtonTitles:[NSString stringWithUTF8String: "Cold (<50°)"],[NSString stringWithUTF8String: "Ambient (50° to 95°)"],[NSString stringWithUTF8String: "Hot (>95°)"], nil];
        
        [temperatureSheet showInView:self.view];
    }
    else
    {
        NSString *message = @"Please enter the "; 
        message = [message stringByAppendingString:[[tableView cellForRowAtIndexPath:indexPath] textLabel].text];
        UIAlertView *inputAlert = [[UIAlertView alloc] 
                                   initWithTitle:[[tableView cellForRowAtIndexPath:indexPath] textLabel].text 
                                   message:message
                                   delegate:self 
                                   cancelButtonTitle:@"Cancel" 
                                   otherButtonTitles:@"Submit", nil];
        [inputAlert setAlertViewStyle:UIAlertViewStylePlainTextInput];
        [inputAlert show];
    }
    
    [tableView deselectRowAtIndexPath:indexPath animated:YES];
}

#pragma mark - Image Picker delegate call backs

- (void)imagePickerController:(UIImagePickerController *)imagePicker didFinishPickingMediaWithInfo:(NSDictionary *)info {
    
    targetImage = [info objectForKey:UIImagePickerControllerOriginalImage];
    
    //Place the Image in CoreData.
    [self.detailItem setValue:UIImagePNGRepresentation(targetImage) forKeyPath:@"test_Photo.image"];
    
    [self.detailDescriptionTable cellForRowAtIndexPath:selectedIndexPath].imageView.image = targetImage;
    
//    [self.detailDescriptionTable reloadData];
    
    [self dismissModalViewControllerAnimated:YES];
}

#pragma mark - Alert View Delegate call backs

- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex
{
    if(buttonIndex == 1)
    {
        [[self.detailDescriptionTable cellForRowAtIndexPath:selectedIndexPath].detailTextLabel setText:[alertView textFieldAtIndex:0].text];
        
        [self updateCoreDataModelWithString:[alertView textFieldAtIndex:0].text atCellIndexPath:selectedIndexPath];
        
        [self.detailDescriptionTable reloadData];
    }
}

- (void)updateCoreDataModelWithString:(NSString *)text atCellIndexPath:(NSIndexPath *)indexPath
{
    switch (indexPath.section) {
        case 1:
            if (indexPath.row == 0)
                [self.detailItem setValue:text forKeyPath:@"test_Shooter.first_Name"];
            else
                [self.detailItem setValue:text forKeyPath:@"test_Shooter.last_Name"];
            break;
        case 2:
            if (indexPath.row == 0)
                [self.detailItem setValue:text forKeyPath:@"test_Range.firing_Range"];
            else if (indexPath.row == 1)
                [self.detailItem setValue:[NSNumber numberWithInt:[text intValue]] forKeyPath:@"test_Range.distance_To_Target"];
            else
                [self.detailItem setValue:[NSNumber numberWithDouble:[text doubleValue]] forKeyPath:@"test_Range.range_Temperature"];
            break;
        case 3:
            if (indexPath.row == 0)
                [self.detailItem setValue:[NSNumber numberWithInt:[text intValue]] forKeyPath:@"test_Weapon.serial_Number"];
            else if (indexPath.row == 1)
                [self.detailItem setValue:text forKeyPath:@"test_Weapon.weapon_Nomenclature"];
            else
                [self.detailItem setValue:text forKeyPath:@"test_Weapon.weapon_Notes"];
            break;
        case 4:
            if (indexPath.row == 0)
                [self.detailItem setValue:[NSNumber numberWithDouble:[text doubleValue]] forKeyPath:@"test_Ammunition.caliber"];
            else if (indexPath.row == 1)
                [self.detailItem setValue:[NSNumber numberWithInt:[text intValue]] forKeyPath:@"test_Ammunition.lot_Number"];
            else if (indexPath.row == 2)
                [self.detailItem setValue:[NSNumber numberWithInt:[text intValue]] forKeyPath:@"test_Ammunition.number_Of_Shots"];
            else if (indexPath.row == 3)
                [self.detailItem setValue:[NSNumber numberWithDouble:[text doubleValue]] forKeyPath:@"test_Ammunition.projectile_Mass"];
            else
                [self.detailItem setValue:text forKeyPath:@"test_Ammunition.ammunition_Notes"];
            break;
        default:
            break;
    }
}

#pragma mark - Split view

- (void)splitViewController:(UISplitViewController *)splitController willHideViewController:(UIViewController *)viewController withBarButtonItem:(UIBarButtonItem *)barButtonItem forPopoverController:(UIPopoverController *)popoverController
{
    barButtonItem.title = NSLocalizedString(@"Master", @"Master");
    [self.navigationItem setLeftBarButtonItem:barButtonItem animated:YES];
    self.masterPopoverController = popoverController;
}

- (void)splitViewController:(UISplitViewController *)splitController willShowViewController:(UIViewController *)viewController invalidatingBarButtonItem:(UIBarButtonItem *)barButtonItem
{
    // Called when the view is shown again in the split view, invalidating the button and popover controller.
    [self.navigationItem setLeftBarButtonItem:nil animated:YES];
    self.masterPopoverController = nil;
}

#pragma mark - Action Sheet Delegate

- (void)actionSheet:(UIActionSheet *)actionSheet clickedButtonAtIndex:(NSInteger)buttonIndex
{
    NSArray *arrayOfTemps = [NSArray arrayWithObjects:[NSString stringWithUTF8String: "Cold (<50°)"],[NSString stringWithUTF8String: "Ambient (50° to 95°)"],[NSString stringWithUTF8String: "Hot (>95°)"], nil];
    switch (buttonIndex) {
        case 0:
            if(selectedIndexPath.section == 2 && selectedIndexPath.row == 2)
                [self.detailItem setValue:[arrayOfTemps objectAtIndex:0] forKeyPath:@"test_Range.range_Temperature"];
            break;
        case 1:
            if(selectedIndexPath.section == 2 && selectedIndexPath.row == 2)
                [self.detailItem setValue:[arrayOfTemps objectAtIndex:1] forKeyPath:@"test_Range.range_Temperature"];
            break;
        case 2:
            if(selectedIndexPath.section == 2 && selectedIndexPath.row == 2)
                [self.detailItem setValue:[arrayOfTemps objectAtIndex:2] forKeyPath:@"test_Range.range_Temperature"];
            break;
        default:
            break;
    }
    [self.detailDescriptionTable reloadData];
}

@end
