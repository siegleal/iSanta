//
//  NewTestViewController.m
//  iSanta
//
//  Created by Jack Hall on 1/10/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "NewTestViewController.h"

@implementation NewTestViewController

@synthesize reportTable;
@synthesize cancelBB;
@synthesize doneBB;
@synthesize context;
@synthesize testReportObject;
@synthesize shooterObject;
@synthesize rangeObject;
@synthesize weaponObject;
@synthesize ammoObject;
@synthesize photoObject;
@synthesize selectedIndexPath;
@synthesize targetImage;
@synthesize fetchedResultsController;

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
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

-(IBAction)dismissNewTestModalView:(id)sender
{
    [self dismissModalViewControllerAnimated:YES];
}

-(IBAction)cancelNewTest:(id)sender
{
    //Remove the entities that were added.(Will Cascade)
    [context deleteObject:testReportObject];
    
    // Save the context.
    NSError *error = nil;
    if (![context save:&error]) {
        /*
         Replace this implementation with code to handle the error appropriately.
         
         abort() causes the application to generate a crash log and terminate. You should not use this function in a shipping application, although it may be useful during development. 
         */
        NSLog(@"Unresolved error %@, %@", error, [error userInfo]);
        //abort();
    }
    
    [self dismissNewTestModalView:self];
}

-(IBAction)saveNewTest:(id)sender
{
    // Save the context.
    NSError *error = nil;
    if (![context save:&error]) {
        /*
         Replace this implementation with code to handle the error appropriately.
         
         abort() causes the application to generate a crash log and terminate. You should not use this function in a shipping application, although it may be useful during development. 
         */
        NSLog(@"Unresolved error %@, %@", error, [error userInfo]);
        //abort();
    }
    
    [self dismissNewTestModalView:self];
}

#pragma mark - View lifecycle

- (void)viewDidLoad
{
    [super viewDidLoad];
    // Do any additional setup after loading the view from its nib.
}

- (void)viewDidUnload
{
    [super viewDidUnload];
    // Release any retained subviews of the main view.
    // e.g. self.myOutlet = nil;
}

- (void)viewDidAppear:(BOOL)animated
{
    [super viewDidAppear:animated];
    
    // Create a new instance of the entity managed by the fetched results controller.
    self.context = [self.fetchedResultsController managedObjectContext];
    NSEntityDescription *entity = [[self.fetchedResultsController fetchRequest] entity];
    testReportObject = [NSEntityDescription insertNewObjectForEntityForName:[entity name] inManagedObjectContext:context];
    
    // If appropriate, configure the new managed object.
    // Normally you should use accessor methods, but using KVC here avoids the need to add a custom class to the template.
    [testReportObject setValue:[NSDate date] forKey:@"date_Time"];
    
    shooterObject = [NSEntityDescription insertNewObjectForEntityForName:@"Shooter_Information" inManagedObjectContext:context];
    [testReportObject setValue:shooterObject forKey:@"test_Shooter"];
    
    rangeObject = [NSEntityDescription insertNewObjectForEntityForName:@"Range_Information" inManagedObjectContext:context];
    [testReportObject setValue:rangeObject forKey:@"test_Range"];
    
    weaponObject = [NSEntityDescription insertNewObjectForEntityForName:@"Weapon_Information" inManagedObjectContext:context];
    [testReportObject setValue:weaponObject forKey:@"test_Weapon"];
    
    ammoObject = [NSEntityDescription insertNewObjectForEntityForName:@"Ammunition_Information" inManagedObjectContext:context];
    [testReportObject setValue:ammoObject forKey:@"test_Ammunition"];
    
    photoObject = [NSEntityDescription insertNewObjectForEntityForName:@"Photo_Information" inManagedObjectContext:context];
    [testReportObject setValue:photoObject forKey:@"test_Photo"];
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
    // Return YES for supported orientations
    return (interfaceOrientation == UIInterfaceOrientationPortrait);
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
        cell = [[UITableViewCell alloc] initWithStyle:UITableViewCellStyleSubtitle reuseIdentifier:CellIdentifier];
    }
    
    switch (indexPath.section) {
        case 0:
              [cell.imageView setImage:[[UIImage alloc] initWithData:
                                        [self.testReportObject valueForKeyPath:@"test_Photo.image"]]];
            [cell.textLabel setText:@"Photo"];
            [cell.detailTextLabel setText:@""];
            break;
        case 1:
            switch (indexPath.row) {
                case 0:
                    [cell.textLabel setText:@"First Name"];
                      [cell.detailTextLabel setText:[[self.testReportObject 
                                                      valueForKeyPath:@"test_Shooter.first_Name"]
                                                     description]];
                    break;
                case 1:
                    [cell.textLabel setText:@"Last Name"];
                      [cell.detailTextLabel setText:[[self.testReportObject 
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
                      [cell.detailTextLabel setText:[[self.testReportObject 
                                                      valueForKeyPath:@"test_Range.firing_Range"]
                                                     description]];
                    break;
                case 1:
                    [cell.textLabel setText:@"Distance to Target"];
                      [cell.detailTextLabel setText:[[self.testReportObject 
                                                      valueForKeyPath:@"test_Range.distance_To_Target"]
                                                     description]];
                    break;
                case 2:
                    [cell.textLabel setText:@"Range Temperature"];
                      [cell.detailTextLabel setText:[[self.testReportObject valueForKeyPath:
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
                      [cell.detailTextLabel setText:[[self.testReportObject 
                                                      valueForKeyPath:@"test_Weapon.serial_Number"]
                                                      description]];
                    break;
                case 1:
                    [cell.textLabel setText:@"Weapon Nomenclature"];
                      [cell.detailTextLabel setText:[[self.testReportObject valueForKeyPath:
                                                      @"test_Weapon.weapon_Nomenclature"]
                                                     description]];
                    break;
                case 2:
                    [cell.textLabel setText:@"Notes"];
                      [cell.detailTextLabel setText:[[self.testReportObject 
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
                      [cell.detailTextLabel setText:[[self.testReportObject 
                                                      valueForKeyPath:@"test_Ammunition.caliber"]
                                                     description]];
                    break;
                case 1:
                    [cell.textLabel setText:@"Lot Number"];
                      [cell.detailTextLabel setText:[[self.testReportObject 
                                                      valueForKeyPath:@"test_Ammunition.lot_Number"]
                                                     description]];
                    break;
                case 2:
                    [cell.textLabel setText:@"Number of Shots Fired"];
                      [cell.detailTextLabel setText:[[self.testReportObject valueForKeyPath:
                                                      @"test_Ammunition.number_Of_Shots"]
                                                     description]];
                    break;
                case 3:
                    [cell.textLabel setText:@"Projectile Mass"];
                      [cell.detailTextLabel setText:[[self.testReportObject valueForKeyPath:
                                                      @"test_Ammunition.projectile_Mass"]
                                                     description]];
                    break;
                case 4:
                    [cell.textLabel setText:@"Notes"];
                      [cell.detailTextLabel setText:[[self.testReportObject valueForKeyPath:
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

#pragma mark - Table View Delegate

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
        UIActionSheet *temperatureSheet = [[UIActionSheet alloc] initWithTitle:@"Choose a Temperature" delegate:self cancelButtonTitle:@"Cancel" destructiveButtonTitle:nil otherButtonTitles:[NSString stringWithUTF8String: "Cold (<50°)"],[NSString stringWithUTF8String: "Ambient (50° to 95°)"],[NSString stringWithUTF8String:"Hot (>95°)"], nil];
        
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
    [testReportObject setValue:UIImageJPEGRepresentation(targetImage, 0.85) forKeyPath:@"test_Photo.image"];
    
    [reportTable cellForRowAtIndexPath:selectedIndexPath].imageView.image = targetImage;
    
    [self dismissModalViewControllerAnimated:YES];
}

#pragma mark - Action Sheet Delegate

- (void)actionSheet:(UIActionSheet *)actionSheet clickedButtonAtIndex:(NSInteger)buttonIndex
{
    NSArray *arrayOfTemps = [NSArray arrayWithObjects:[NSString stringWithUTF8String: "Cold (<50°)"],[NSString stringWithUTF8String: "Ambient (50° to 95°)"],[NSString stringWithUTF8String: "Hot (>95°)"], nil];
    switch (buttonIndex) {
        case 0:
            if(selectedIndexPath.section == 2 && selectedIndexPath.row == 2)
                [testReportObject setValue:[arrayOfTemps objectAtIndex:0] forKeyPath:@"test_Range.range_Temperature"];
            break;
        case 1:
            if(selectedIndexPath.section == 2 && selectedIndexPath.row == 2)
                [testReportObject setValue:[arrayOfTemps objectAtIndex:1] forKeyPath:@"test_Range.range_Temperature"];
            break;
        case 2:
            if(selectedIndexPath.section == 2 && selectedIndexPath.row == 2)
                [testReportObject setValue:[arrayOfTemps objectAtIndex:2] forKeyPath:@"test_Range.range_Temperature"];
            break;
        default:
            break;
    }
    [self.reportTable reloadData];
}

#pragma mark - Alert View Delegate call backs

- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex
{
    if(buttonIndex == 1)
    {
        [[self.reportTable cellForRowAtIndexPath:selectedIndexPath].detailTextLabel setText:[alertView textFieldAtIndex:0].text];
        
        [self updateCoreDataModelWithString:[alertView textFieldAtIndex:0].text atCellIndexPath:selectedIndexPath];
        
        [self.reportTable reloadData];
    }
}

- (void)updateCoreDataModelWithString:(NSString *)text atCellIndexPath:(NSIndexPath *)indexPath
{
    switch (indexPath.section) {
        case 1:
            if (indexPath.row == 0)
                [testReportObject setValue:text forKeyPath:@"test_Shooter.first_Name"];
            else
                [testReportObject setValue:text forKeyPath:@"test_Shooter.last_Name"];
            break;
        case 2:
            if (indexPath.row == 0)
                [testReportObject setValue:text forKeyPath:@"test_Range.firing_Range"];
            else if (indexPath.row == 1)
                [testReportObject setValue:[NSNumber numberWithInt:[text intValue]] forKeyPath:@"test_Range.distance_To_Target"];
            else
                [testReportObject setValue:[NSNumber numberWithDouble:[text doubleValue]] forKeyPath:@"test_Range.range_Temperature"];
            break;
        case 3:
            if (indexPath.row == 0)
                [testReportObject setValue:[NSNumber numberWithInt:[text intValue]] forKeyPath:@"test_Weapon.serial_Number"];
            else if (indexPath.row == 1)
                [testReportObject setValue:text forKeyPath:@"test_Weapon.weapon_Nomenclature"];
            else
                [testReportObject setValue:text forKeyPath:@"test_Weapon.weapon_Notes"];
            break;
        case 4:
            if (indexPath.row == 0)
                [testReportObject setValue:[NSNumber numberWithDouble:[text doubleValue]] forKeyPath:@"test_Ammunition.caliber"];
            else if (indexPath.row == 1)
                [testReportObject setValue:[NSNumber numberWithInt:[text intValue]] forKeyPath:@"test_Ammunition.lot_Number"];
            else if (indexPath.row == 2)
                [testReportObject setValue:[NSNumber numberWithInt:[text intValue]] forKeyPath:@"test_Ammunition.number_Of_Shots"];
            else if (indexPath.row == 3)
                [testReportObject setValue:[NSNumber numberWithDouble:[text doubleValue]] forKeyPath:@"test_Ammunition.projectile_Mass"];
            else
                [testReportObject setValue:text forKeyPath:@"test_Ammunition.ammunition_Notes"];
            break;
        default:
            break;
    }
}



@end
