//
//  NewTestTableViewController.m
//  iSANTA
//
//  Created by Jack Hall on 10/22/11.
//  Copyright (c) 2011 __MyCompanyName__. All rights reserved.
//

#import "NewTestTableViewController.h"


@implementation NewTestTableViewController

@synthesize targetImage;
@synthesize selectedIndexPath;
@synthesize arrayTemps;
@synthesize tempPickerViewController;

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

-(IBAction)dismissNewTestModalView:(id)sender
{
    [[self parentViewController] dismissModalViewControllerAnimated:YES];
}

#pragma mark - View lifecycle

- (void)viewDidLoad
{
    [super viewDidLoad];

    // Uncomment the following line to preserve selection between presentations.
    // self.clearsSelectionOnViewWillAppear = NO;
 
    // Uncomment the following line to display an Edit button in the navigation bar for this view controller.
    // self.navigationItem.rightBarButtonItem = self.editButtonItem;
    
    arrayTemps = [[NSMutableArray alloc] init];
    [arrayTemps addObject:@"Cold (Temp < 50 degrees)"];
    [arrayTemps addObject:@"Ambient (50 degrees < Temp < 95 degrees)"];
    [arrayTemps addObject:@"Hot (95 degrees < Temp)"];
    
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

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section
{
    // Return the number of rows in the section.
    if(section == 0)
        return 1;
    else
        return 4;
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath
{
    static NSString *CellIdentifier = @"Cell";
    
    UITableViewCell *cell = [tableView dequeueReusableCellWithIdentifier:CellIdentifier];
    if (cell == nil) {
        if(indexPath.section == 0)
        {
            cell = [[UITableViewCell alloc] initWithStyle:UITableViewCellStyleDefault reuseIdentifier:CellIdentifier];
        }
        /*else if(indexPath.section == 1 && indexPath.row >= 2 && indexPath.row <= 3)
        {
            cell = [[UITableViewCell alloc] initWithStyle:UITableViewCellStyleDefault reuseIdentifier:CellIdentifier];
        }*/
        else
        {
            cell = [[UITableViewCell alloc] initWithStyle:UITableViewCellStyleSubtitle reuseIdentifier:CellIdentifier];
        }
    }
    
    // Configure the cell...
    
    [cell setAccessoryType:UITableViewCellAccessoryDisclosureIndicator];
    
    if(indexPath.section == 0)
    {
        switch (indexPath.row) {
            case 0:
                [cell.textLabel setText:@"Target Photo"];
                [cell.imageView setImage:targetImage];
                break;
                
            default:
                break;
        }
    }
    else if(indexPath.section == 1)
    {
        switch (indexPath.row) {
            case 0:
                [cell.textLabel setText:@"Shooter Name"];
                break;
            case 1:
                [cell.textLabel setText:@"Firing Range"];
                break;
            case 2:
                [cell.textLabel setText:@"Distance to Target"];
                break;
            case 3:
                [cell.textLabel setText:@"Range Temperature"];
                break;
            default:
                break;
        }
    }
    else if(indexPath.section == 2)
    {
        switch (indexPath.row) {
            case 0:
                [cell.textLabel setText:@"Weapon Nomeclature"];
                break;
            case 1:
                [cell.textLabel setText:@"Serial Number"];
                break;
            case 2:
                [cell.textLabel setText:@"Weapon Notes"];
                break;
            case 3:
                [cell.textLabel setText:@"Ammunition Data"];
                break;
            default:
                break;
        }
    }
    
    return cell;
}

#pragma mark - Table view delegate

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
    else if(indexPath.section == 1 && indexPath.row == 3)
    {
        tempPickerViewController = [[PickerViewController alloc] initWithItemArray:arrayTemps tableView:self.tableView indexPath:indexPath];
        [UIView animateWithDuration:0.3 animations:^{
            tempPickerViewController.view.center = CGPointMake(160, tempPickerViewController.view.bounds.size.height);
            [self.view addSubview:tempPickerViewController.view];
            tempPickerViewController.view.center = CGPointMake(160, (tempPickerViewController.view.bounds.size.height/4)+72);
        }];
    }
    else if(indexPath.section == 2 && indexPath.row >= 2 && indexPath.row <= 3)
    {
        
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
    [self.tableView reloadData];
    
    [self dismissModalViewControllerAnimated:YES];
}

#pragma mark - Alert View Delegate call backs

- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex
{
    if(buttonIndex == 1)
    {
        [[self.tableView cellForRowAtIndexPath:selectedIndexPath].detailTextLabel setText:[alertView textFieldAtIndex:0].text];
        [self.tableView reloadData];
    }
}

@end
