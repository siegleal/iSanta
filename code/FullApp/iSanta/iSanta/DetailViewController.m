//
//  DetailViewController.m
//  iSanta
//
//  Created by Jack Hall on 12/14/11.
//  Copyright (c) 2011 __MyCompanyName__. All rights reserved.
//

#import "DetailViewController.h"
#import <objc/runtime.h>

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
@synthesize doneToolBar;
@synthesize temperaturePicker;
@synthesize respondingTextField;
@synthesize pickerController;
@synthesize manPlace;
@synthesize points;

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
    
    temperaturePicker = [[UIPickerView alloc] initWithFrame:CGRectMake(0, 200, 320, 216)];
    temperaturePicker.delegate = self;
    temperaturePicker.showsSelectionIndicator = YES;
    
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
//    [self.view setBackgroundColor:[UIColor colorWithPatternImage:[UIImage imageNamed:@"iSantaTableBackground.png"]]];
    [super viewWillAppear:animated];
}

- (void)viewDidAppear:(BOOL)animated
{
    [super viewDidAppear:animated];
}

- (void)viewWillDisappear:(BOOL)animated
{
    [self updateCoreDataModelWithString:nil atCellIndexPath:[NSIndexPath indexPathForRow:0 inSection:0]];
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

- (void)saveExportImage:(NSData *)imageData
{
    [self.detailItem setValue:imageData forKeyPath:@"test_Photo.export_Image"];
}

- (void)prepareForSegue:(UIStoryboardSegue *)segue sender:(id)sender
{
    if ([[segue identifier] isEqualToString:@"detailToStats"])
    {
        //Send points to the stats controller.
        StatsDisplayController *statsDisplayController = [segue destinationViewController];
        [statsDisplayController setPoints:self.points];
        //Send photo with points to stats controller.
        [statsDisplayController setTargetPhoto:[self.detailItem valueForKeyPath:@"test_Photo.export_Image"]];
        //Create the dictionary of report data.
        NSMutableDictionary *reportDictionary = [[NSMutableDictionary alloc] init];
        //Get values for the data.
        NSString *name = [NSString stringWithFormat:@"%s, %s",[self.detailItem valueForKeyPath:@"test_Shooter.last_Name"],[self.detailItem valueForKeyPath:@"test_Shooter.first_Name"]];
        
        NSLocale *currentLocale = [NSLocale systemLocale];
        NSString *dateFormat;
        NSString *dateComponents = @"MMM dd, yyyy at HH:mm ZZZZ";
        dateFormat = [NSDateFormatter dateFormatFromTemplate:dateComponents options:0 locale:currentLocale];
        NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
        [dateFormatter setDateFormat:dateFormat];
        NSString *formattedDate = [dateFormatter stringFromDate:(NSDate *)[self.detailItem valueForKey:@"date_Time"]];
        
        NSString *range = [self.detailItem valueForKeyPath:@"test_Range.firing_Range"];
        NSString *temp = [self.detailItem valueForKeyPath:@"test_Range.range_Temperature"];
        NSString *distance = [self.detailItem valueForKeyPath:@"test_Range.distance_To_Target"];
        NSString *numShots = [self.detailItem valueForKeyPath:@"test_Ammunition.number_Of_Shots"];
        NSString *weaponSerialNumber = [self.detailItem valueForKeyPath:@"test_Weapon.serial_Number"];
        NSString *nomenclature = [self.detailItem valueForKeyPath:@"test_Weapon.weapon_Nomenclature"];
        NSString *caliber = [self.detailItem valueForKeyPath:@"test_Ammunition.caliber"];
        NSString *lotNum = [self.detailItem valueForKeyPath:@"test_Ammunition.lot_Number"];
        NSString *mass = [self.detailItem valueForKeyPath:@"test_Ammunition.projectile_Mass"];
        if(name != nil && formattedDate != nil && range != nil && temp != nil && distance != nil && numShots != nil && weaponSerialNumber != nil && nomenclature != nil && caliber != nil && lotNum != nil && mass != nil)
        {
            //Shooter Name (last, first)
            [reportDictionary setObject:name forKey:@"Shooter"];
            //Test Date
            [reportDictionary setObject:formattedDate forKey:@"Date Fired"];
            //Firing Range
            [reportDictionary setObject:range forKey:@"Place"];
            //Temperature
            [reportDictionary setObject:temp forKey:@"Temperature"];
            //Target Distance
            [reportDictionary setObject:distance  forKey:@"Target Distance"];
            //Shots Fired
            [reportDictionary setObject:numShots forKey:@"Shots Fired"];
            //Serial Number
            [reportDictionary setObject:weaponSerialNumber forKey:@"Weapon Serial #"];
            //Weapon Nomenclature
            [reportDictionary setObject:nomenclature forKey:@"Weapon Nomenclature"];
            //Projectile Caliber
            [reportDictionary setObject:caliber forKey:@"Projectile Caliber"];
            //Lot Number
            [reportDictionary setObject:lotNum forKey:@"Lot #"];
            //Projectile Mass
            [reportDictionary setObject:mass forKey:@"Projectile Mass"];
            //send the dictionary to the stats controller.
            [statsDisplayController setReportData:reportDictionary];
        }
        else
        {
            UIAlertView *error = [[UIAlertView alloc] initWithTitle:@"Warning" message:@"There are missing values in this report. Please be sure to fill out all of the values in the report before trying to export." delegate:nil cancelButtonTitle:@"OK" otherButtonTitles: nil];
            [error show];
        }
    }
}

#pragma mark - Point Methods


- (NSMutableArray *)getArrayOfPointsFromDetailItem:(id)detailItem
{
    points = [NSKeyedUnarchiver unarchiveObjectWithData:[self.detailItem valueForKeyPath:@"test_Photo.points"]];
    if (points == nil) {
        points = [[NSMutableArray alloc] init];
    }
    return points;
}

- (void)addPointWithXValue:(int)x andYValue:(int)y
{
    //Add a point locally.
    [self.points addObject:[NSValue valueWithCGPoint:CGPointMake(x, y)]];
    //Save points to CoreData.
    [self.detailItem setValue:[NSKeyedArchiver archivedDataWithRootObject:self.points] forKeyPath:@"test_Photo.points"];
}

- (void)setPoints:(NSMutableArray *)inPoints
{
    self.points = inPoints;
    [self.detailItem setValue:[NSKeyedArchiver archivedDataWithRootObject:self.points] forKeyPath:@"test_Photo.points"];
}

#pragma mark - Unit Conversion Methods

- (double)convertInchesToCentimeters:(double)inches 
{
    return inches * 2.54;
}

- (double)convertCentimetersToInches:(double)centimeters
{
    return centimeters * 0.3937;
}

- (double)convertYardsToMeters:(double)yards
{
    return yards * 0.9144;
}

- (double)convertMetersToYards:(double)meters
{
    return meters * 1.094;
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
    DetailTableViewCell *cell = [[DetailTableViewCell alloc] initWithStyle:UITableViewCellStyleSubtitle reuseIdentifier:nil indexPath:indexPath];
    
    cell.textField.delegate = self;
    
    doneToolBar = [[UIToolbar alloc] initWithFrame:CGRectMake(0, 0, 320, 44)];
    [doneToolBar setBarStyle:UIBarStyleBlackOpaque];
    UIBarButtonItem *fixedWidth = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemFixedSpace target:nil action:nil];
    fixedWidth.width = doneToolBar.frame.size.width - 70;
    UIBarButtonItem *done = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemDone target:cell.textField action:@selector(resignFirstResponder)];
    
    [doneToolBar setItems:[NSArray arrayWithObjects:fixedWidth, done, nil] animated:NO];
    
    [cell.textField setInputAccessoryView:doneToolBar];
    
    [cell setSelectionStyle:UITableViewCellSelectionStyleNone];
    
    NSString *value = @"";
    
    BOOL metric = [[NSUserDefaults standardUserDefaults] boolForKey:@"unit_preference"];
    
    if (cell == nil) {
        cell = [[DetailTableViewCell alloc] initWithStyle:UITableViewCellStyleSubtitle reuseIdentifier:nil indexPath:indexPath];
    }
    
    switch (indexPath.section) {
        case 0:
            switch (indexPath.row) {
                case 0:
                    [cell setSelectionStyle:UITableViewCellSelectionStyleBlue];
                    [cell.imageView setImage:[[UIImage alloc] initWithData:
                                              [self.detailItem valueForKeyPath:@"test_Photo.image"]]];
                    self.targetImage = [UIImage imageWithData:[self.detailItem valueForKeyPath:@"test_Photo.image"]];
                    cell.textField.enabled = NO;
                    [cell.textLabel setText:@"Photo"];
                    [cell.detailTextLabel setText:@""];
                    break;
                case 1:                    
                    [cell.imageView setImage:nil];
                    value = [[self.detailItem valueForKeyPath:@"test_Photo.target_Height"]description];
                    if (metric) {
                        [cell setTextFieldPlaceholder:@"Target Height (cm)"];
                        value = [NSString stringWithFormat:@"%f",[self convertInchesToCentimeters:[value doubleValue]]];
                    }
                    else {
                        [cell setTextFieldPlaceholder:@"Target Height (in)"];
                    }
                    if ([value doubleValue] != 0.0)
                        [cell setTextFieldText:value];
                    else
                        [cell setTextFieldText:@""];
                    cell.textField.keyboardType = UIKeyboardTypeNumbersAndPunctuation;
                    break;
                case 2:
                    [cell.imageView setImage:nil];
                    value = [[self.detailItem valueForKeyPath:@"test_Photo.target_Width"]description];
                    if (metric) {
                        [cell setTextFieldPlaceholder:@"Target Width (cm)"];
                        value = [NSString stringWithFormat:@"%f",[self convertInchesToCentimeters:[value doubleValue]]];
                    }
                    else {
                        [cell setTextFieldPlaceholder:@"Target Width (in)"];
                    }                    
                    if ([value doubleValue] != 0.0)
                        [cell setTextFieldText:value];
                    else
                        [cell setTextFieldText:@""];
                    cell.textField.keyboardType = UIKeyboardTypeNumbersAndPunctuation;
                    break;
                default:
                    break;
            }
            
            break;
        case 1:
            switch (indexPath.row) {
                case 0:
                    [cell.imageView setImage:nil];
                    [cell setTextFieldPlaceholder:@"First Name"];
                    [cell setTextFieldText:[[self.detailItem valueForKeyPath:@"test_Shooter.first_Name"] description]];
                    cell.textField.keyboardType = UIKeyboardTypeASCIICapable;
                    break;
                case 1:
                    [cell.imageView setImage:nil];
                    [cell setTextFieldPlaceholder:@"Last Name"];
                    [cell setTextFieldText:[[self.detailItem valueForKeyPath:@"test_Shooter.last_Name"]description]];
                    cell.textField.keyboardType = UIKeyboardTypeASCIICapable;
                    break;
                default:
                    break;
            }
            break;
        case 2:
            switch (indexPath.row) {
                case 0:
                    [cell.imageView setImage:nil];
                    [cell setTextFieldPlaceholder:@"Firing Range"];
                    [cell setTextFieldText:[[self.detailItem valueForKeyPath:@"test_Range.firing_Range"]description]];
                    cell.textField.keyboardType = UIKeyboardTypeASCIICapable;
                    break;
                case 1:
                    [cell.imageView setImage:nil];
                    value = [[self.detailItem valueForKeyPath:@"test_Range.distance_To_Target"] description];
                    if (metric) {
                        [cell setTextFieldPlaceholder:@"Distance to Target (m)"];
                        value = [NSString stringWithFormat:@"%f",[self convertYardsToMeters:[value doubleValue]]];
                    }
                    else {
                        [cell setTextFieldPlaceholder:@"Distance to Target (yd)"];
                    }
                    if ([value doubleValue] != 0.0)
                        [cell setTextFieldText:value];
                    else
                        [cell setTextFieldText:@""];
                    cell.textField.keyboardType = UIKeyboardTypeNumberPad;
                    break;
                case 2:
                    [cell.imageView setImage:nil];
                    [cell setTextFieldPlaceholder:@"Range Temperature"];
                    value = [[self.detailItem valueForKeyPath:@"test_Range.range_Temperature"] description];
                    if (value != nil && [value length] > 0)
                        [cell setTextFieldText:value];
                    else
                        [cell setTextFieldText:@""];
                    [cell.textField setInputView:temperaturePicker];
                    break;
                default:
                    break;
            }
            break;
        case 3:
            switch (indexPath.row) {
                case 0:
                    [cell.imageView setImage:nil];
                    [cell setTextFieldPlaceholder:@"Serial Number"];
                    value = [[self.detailItem valueForKeyPath:@"test_Weapon.serial_Number"]description];
                    if (![value isEqualToString:@"0"])
                        [cell setTextFieldText:value];
                    else
                        [cell setTextFieldText:@""];
                    cell.textField.keyboardType = UIKeyboardTypeNumberPad;
                    break;
                case 1:
                    [cell.imageView setImage:nil];
                    [cell setTextFieldPlaceholder:@"Weapon Nomenclature"];
                    [cell setTextFieldText:[[self.detailItem valueForKeyPath:@"test_Weapon.weapon_Nomenclature"] description]];
                    cell.textField.keyboardType = UIKeyboardTypeASCIICapable;
                    break;
                case 2:
                    [cell.imageView setImage:nil];
                    [cell setTextFieldPlaceholder:@"Notes"];
                    [cell setTextFieldText:[[self.detailItem valueForKeyPath:@"test_Weapon.weapon_Notes"]description]];
                    cell.textField.keyboardType = UIKeyboardTypeASCIICapable;
                    break;
                default:
                    break;
            }
            break;
        case 4:
            switch (indexPath.row) {
                case 0:
                    [cell.imageView setImage:nil];
                    [cell setTextFieldPlaceholder:@"Caliber"];
                    value = [[self.detailItem valueForKeyPath:@"test_Ammunition.caliber"]description];
                    if (![value isEqualToString:@"0"])
                        [cell setTextFieldText:value];
                    else
                        [cell setTextFieldText:@""];
                    cell.textField.keyboardType = UIKeyboardTypeNumberPad;
                    break;
                case 1:
                    [cell.imageView setImage:nil];
                    [cell setTextFieldPlaceholder:@"Lot Number"];
                    value = [[self.detailItem valueForKeyPath:@"test_Ammunition.lot_Number"] description];
                    if (![value isEqualToString:@"0"])
                        [cell setTextFieldText:value];
                    else
                        [cell setTextFieldText:@""];
                    cell.textField.keyboardType = UIKeyboardTypeNumberPad;
                    break;
                case 2:
                    [cell.imageView setImage:nil];
                    [cell setTextFieldPlaceholder:@"Number of Shots Fired"];
                    value = [[self.detailItem valueForKeyPath:@"test_Ammunition.number_Of_Shots"] description];
                    if (![value isEqualToString:@"0"])
                        [cell setTextFieldText:value];
                    else
                        [cell setTextFieldText:@""];
                    cell.textField.keyboardType = UIKeyboardTypeNumberPad;
                    break;
                case 3:
                    [cell.imageView setImage:nil];
                    [cell setTextFieldPlaceholder:@"Projectile Mass"];
                    value = [[self.detailItem valueForKeyPath:@"test_Ammunition.projectile_Mass"] description];
                    if (![value isEqualToString:@"0"])
                        [cell setTextFieldText:value];
                    else
                        [cell setTextFieldText:@""];
                    cell.textField.keyboardType = UIKeyboardTypeNumberPad;
                    break;
                case 4:
                    [cell.imageView setImage:nil];
                    [cell setTextFieldPlaceholder:@"Notes"];
                    [cell setTextFieldText:[[self.detailItem valueForKeyPath:@"test_Ammunition.ammunition_Notes"] description]];
                    cell.textField.keyboardType = UIKeyboardTypeASCIICapable;

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
            return 3;
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
            return @"Ammunition Informtion";
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
////////////////////////    
//    [tableView scrollToRowAtIndexPath:indexPath atScrollPosition:UITableViewScrollPositionMiddle animated:YES];
////////////////////////    
    if(indexPath.section == 0 && indexPath.row == 0)
    {
        UIActionSheet *photoSourceChooser = [[UIActionSheet alloc] initWithTitle:nil delegate:self cancelButtonTitle:@"Cancel" destructiveButtonTitle:nil otherButtonTitles:@"Analyze Current", @"Take Photo", @"Choose Existing", nil];
        [photoSourceChooser showInView:self.view];
    }
    else {
        [tableView scrollToRowAtIndexPath:indexPath atScrollPosition:UITableViewScrollPositionMiddle animated:YES];
    }
/*    else
    {
        [((DetailTableViewCell *)[tableView cellForRowAtIndexPath:indexPath]).textField becomeFirstResponder];
        if (((DetailTableViewCell *)[tableView cellForRowAtIndexPath:indexPath]).textField.isFirstResponder)
        {
            NSLog(@"TextField is first responder");
        }*/
/*      else if(indexPath.section == 2 && indexPath.row == 2)
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
        if(indexPath.section == 1)
        {
            [[inputAlert textFieldAtIndex:0] setKeyboardType:UIKeyboardTypeASCIICapable];
        }
        else if(indexPath.section == 2)
        {
            if(indexPath.row == 0)
                [[inputAlert textFieldAtIndex:0] setKeyboardType:UIKeyboardTypeASCIICapable];
            else
                [[inputAlert textFieldAtIndex:0] setKeyboardType:UIKeyboardTypeNumberPad];
        }
        else if(indexPath.section == 3)
        {
            if(indexPath.row == 0)
                [[inputAlert textFieldAtIndex:0] setKeyboardType:UIKeyboardTypeNumberPad];
            else
                [[inputAlert textFieldAtIndex:0] setKeyboardType:UIKeyboardTypeASCIICapable];
        }
        else if(indexPath.section == 4)
        {
            if(indexPath.row == 4)
                [[inputAlert textFieldAtIndex:0] setKeyboardType:UIKeyboardTypeASCIICapable];
            else
                [[inputAlert textFieldAtIndex:0] setKeyboardType:UIKeyboardTypeNumberPad];
        }
        [inputAlert show];
    }
 */
 /*   }
    [tableView reloadData];
    [tableView deselectRowAtIndexPath:indexPath animated:YES];*/
}

#pragma mark - Image Picker delegate call backs

- (void)imagePickerController:(UIImagePickerController *)imagePicker didFinishPickingMediaWithInfo:(NSDictionary *)info {
    
    targetImage = [info objectForKey:UIImagePickerControllerOriginalImage];
    
    //Place the Image in CoreData.
    [self.detailItem setValue:UIImageJPEGRepresentation(targetImage, 0.85) forKeyPath:@"test_Photo.image"];
    
    [self.detailDescriptionTable cellForRowAtIndexPath:selectedIndexPath].imageView.image = targetImage;
    
    [self.detailDescriptionTable reloadData];
    
    [self dismissModalViewControllerAnimated:YES];
}

#pragma mark - Alert View Delegate call backs
- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex
{
    if(buttonIndex == 1)
    {
        [[self.detailDescriptionTable cellForRowAtIndexPath:selectedIndexPath].detailTextLabel setText:[alertView textFieldAtIndex:0].text];
        
        [self updateCoreDataModelWithString:[alertView textFieldAtIndex:0].text atCellIndexPath:selectedIndexPath];
    }
    [self.detailDescriptionTable reloadData];
}

- (void)updateCoreDataModelWithString:(NSString *)text atCellIndexPath:(NSIndexPath *)indexPath
{
    BOOL metric = [[NSUserDefaults standardUserDefaults] boolForKey:@"unit_preference"];
    
    switch (indexPath.section) {
        case 0:
            if (indexPath.row == 0)
                [self.detailItem setValue:[NSKeyedArchiver archivedDataWithRootObject:self.points] forKeyPath:@"test_Photo.points"];
            else if (indexPath.row == 1)
            {
                if (metric) {//convert cm to inches
                    [self.detailItem setValue:[NSNumber numberWithDouble:[self convertCentimetersToInches:[text doubleValue]]] forKeyPath:@"test_Photo.target_Height"];
                }
                else {//save inches
                    [self.detailItem setValue:[NSNumber numberWithDouble:[text doubleValue]] forKeyPath:@"test_Photo.target_Height"];
                }
            }
            else if (indexPath.row == 2)
            {
                if (metric) {//convert cm to inches
                    [self.detailItem setValue:[NSNumber numberWithDouble:[self convertCentimetersToInches:[text doubleValue]]] forKeyPath:@"test_Photo.target_Width"];
                }
                else {//save inches
                    [self.detailItem setValue:[NSNumber numberWithDouble:[text doubleValue]] forKeyPath:@"test_Photo.target_Width"];
                }
            }
            break;
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
            {
                if (metric) {
                    [self.detailItem setValue:[NSNumber numberWithInt:[self convertMetersToYards:[text doubleValue]]] forKeyPath:@"test_Range.distance_To_Target"];
                }
                [self.detailItem setValue:[NSNumber numberWithInt:[text doubleValue]] forKeyPath:@"test_Range.distance_To_Target"];
            }
            else
                [self.detailItem setValue:text forKeyPath:@"test_Range.range_Temperature"];
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
    pickerController = [[UIImagePickerController alloc] init];
    pickerController.delegate = self;
    
//    NSArray *temp = [NSArray arrayWithObjects:[NSValue valueWithCGPoint:CGPointMake(500, 500)], [NSValue valueWithCGPoint:CGPointMake(600, 600)], [NSValue valueWithCGPoint:CGPointMake(700, 700)], [NSValue valueWithCGPoint:CGPointMake(400, 400)], nil];
      
    switch (buttonIndex) {
        case 0:
            if (self.targetImage == NULL) {
                [[[UIAlertView alloc] initWithTitle:@"No Image Added" message:@"You have not added an image to this report. Please add an image before trying to analyze." delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil] show];
                    break;
            }
            
            manPlace = [[ManualPlacementViewController alloc] initWithNibName:@"ManualPlacement" bundle:nil];
            [manPlace.brain setTargetImage:self.targetImage];
            [manPlace.brain setPoints:[self getArrayOfPointsFromDetailItem:self.detailItem]];
            manPlace.title = @"Target Analysis";
            [manPlace.imageView setImage:self.targetImage];
            manPlace.detailView = self;
            break;
        case 1:
            pickerController.sourceType = UIImagePickerControllerSourceTypeCamera;
            pickerController.cameraDevice = UIImagePickerControllerCameraDeviceRear;
            pickerController.cameraCaptureMode = UIImagePickerControllerCameraCaptureModePhoto;
            break;
        case 2:
            pickerController.sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
            break;
        default:
            break;
    }
    if (buttonIndex == 0)
    {
        [self.navigationController pushViewController:manPlace animated:YES];
    }
    else if (buttonIndex != 3)
    {
        [self presentModalViewController:pickerController animated:YES];
    }
}

#pragma mark - UITextField Delegate
- (BOOL)textFieldShouldBeginEditing:(UITextField *)textField
{
    respondingTextField = textField;
    return YES;
}

- (void)textFieldDidBeginEditing:(UITextField *)textField
{
//    respondingTextField = textField;
    [self.view setFrame:CGRectMake(0, 170, 320, self.detailDescriptionTable.frame.size.height - 280)];
    
    NSLog(@"ASDFSADF");
    
    CGRect r = CGRectMake(textField.bounds.origin.x, textField.bounds.origin.y, textField.bounds.size.width, textField.bounds.size.height);
    r = [self.view convertRect:r fromView:textField.superview];
    //[self.detailDescriptionTable scrollRectToVisible:r animated:YES];
}

- (void)textFieldDidEndEditing:(UITextField *)textField
{
    
    [self.view setFrame:CGRectMake(0, 0, 320, 416)];
    
    switch (textField.tag) {
        case 001:
            [self.detailItem setValue:[NSNumber numberWithDouble:[textField.text doubleValue]] forKeyPath:@"test_Photo.target_Height"];
             NSLog(@"%f",[[[self.detailItem valueForKeyPath:@"test_Photo.target_Height"] description] doubleValue]);
            break;
        case 002:
            [self.detailItem setValue:[NSNumber numberWithDouble:[textField.text doubleValue]]  forKeyPath:@"test_Photo.target_Width"];
            break;
        case 100:
            [self.detailItem setValue:textField.text forKeyPath:@"test_Shooter.first_Name"];
            break;
        case 101:
            [self.detailItem setValue:textField.text forKeyPath:@"test_Shooter.last_Name"];
            break;
        case 200:
            [self.detailItem setValue:textField.text forKeyPath:@"test_Range.firing_Range"];
            break;
        case 201:
            [self.detailItem setValue:[NSNumber numberWithInt:[textField.text intValue]] forKeyPath:@"test_Range.distance_To_Target"];
            break;
        case 202:
            [self.detailItem setValue:textField.text forKeyPath:@"test_Range.range_Temperature"];
            break;
        case 300:
            [self.detailItem setValue:[NSNumber numberWithInt:[textField.text intValue]] forKeyPath:@"test_Weapon.serial_Number"];
            break;
        case 301:
            [self.detailItem setValue:textField.text forKeyPath:@"test_Weapon.weapon_Nomenclature"];
            break;
        case 302:
            [self.detailItem setValue:textField.text forKeyPath:@"test_Weapon.weapon_Notes"];
            break;
        case 400:
            [self.detailItem setValue:[NSNumber numberWithDouble:[textField.text doubleValue]] forKeyPath:@"test_Ammunition.caliber"];
            break;
        case 401:
            [self.detailItem setValue:[NSNumber numberWithInt:[textField.text intValue]] forKeyPath:@"test_Ammunition.lot_Number"];
            break;
        case 402:
            [self.detailItem setValue:[NSNumber numberWithInt:[textField.text intValue]] forKeyPath:@"test_Ammunition.number_Of_Shots"];
            break;
        case 403:
            [self.detailItem setValue:[NSNumber numberWithDouble:[textField.text doubleValue]] forKeyPath:@"test_Ammunition.projectile_Mass"];
            break;
        case 404:
            [self.detailItem setValue:textField.text forKeyPath:@"test_Ammunition.ammunition_Notes"];
            break;
        default:
            break;
    }
    [textField setUserInteractionEnabled:NO];
}

- (BOOL)textFieldShouldReturn:(UITextField *)textField
{
    //[textField resignFirstResponder];
    return NO;
}

- (IBAction)dismissKeyboard:(id)sender
{
    [self.view endEditing:YES];
}

#pragma mark - UIPickerViewDelegate

- (void)pickerView:(UIPickerView *)pickerView didSelectRow: (NSInteger)row inComponent:(NSInteger)component {
    respondingTextField.text = [self pickerView:pickerView titleForRow:row forComponent:component];
}

// tell the picker the title for a given component
- (NSString *)pickerView:(UIPickerView *)pickerView titleForRow:(NSInteger)row forComponent:(NSInteger)component {
    if (row == 0) {
        return [NSString stringWithUTF8String: "Cold (<50°)"];
    }
    else if (row == 1)
    {
        return [NSString stringWithUTF8String: "Ambient (50° to 95°)"];
    }
    else
    {
        return [NSString stringWithUTF8String: "Hot (>95°)"];
    }
}

// tell the picker the width of each row for a given component
- (CGFloat)pickerView:(UIPickerView *)pickerView widthForComponent:(NSInteger)component {
    int sectionWidth = 300;
    
    return sectionWidth;
}

#pragma mark - UIPickerViewDatasource

// tell the picker how many rows are available for a given component
- (NSInteger)pickerView:(UIPickerView *)pickerView numberOfRowsInComponent:(NSInteger)component {
    NSUInteger numRows = 3;
    
    return numRows;
}

// tell the picker how many components it will have
- (NSInteger)numberOfComponentsInPickerView:(UIPickerView *)pickerView {
    return 1;
}

@end
