//
//  PickerViewController.m
//  iSANTA
//
//  Created by Jack Hall on 11/12/11.
//  Copyright (c) 2011 __MyCompanyName__. All rights reserved.
//

#import "PickerViewController.h"

@implementation PickerViewController

@synthesize picker;
@synthesize itemArray;
@synthesize superTable;
@synthesize selectedIndexPath;
@synthesize done;

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Custom initialization
    }
    return self;
}

- (id)initWithItemArray:(NSMutableArray *)inItems tableView:(UITableView *)inTable indexPath:(NSIndexPath *)inPath
{
    self = [super init];
    itemArray = inItems;
    superTable = inTable;
    done.action = @selector(dismissPickerView:);
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
    // Do any additional setup after loading the view from its nib.
}

- (void)viewDidUnload
{
    [super viewDidUnload];
    // Release any retained subviews of the main view.
    // e.g. self.myOutlet = nil;
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
    // Return YES for supported orientations
    return (interfaceOrientation == UIInterfaceOrientationPortrait);
}

-(IBAction)dismissPickerView:(id)sender
{
    [[self.superTable cellForRowAtIndexPath:selectedIndexPath].detailTextLabel setText:[NSString stringWithString:[itemArray objectAtIndex:[self.picker selectedRowInComponent:0]]]];
    //[self dismissModalViewControllerAnimated:YES];
    [UIView animateWithDuration:0.3 animations:^{
        self.view.center = CGPointMake(160, self.view.bounds.size.height);
        [self.view removeFromSuperview];
        [self removeFromParentViewController];
     }];
}

#pragma mark - Picker View Delegate call backs

- (NSInteger)numberOfComponentsInPickerView:(UIPickerView *)thePickerView 
{             
    return 1;
}

- (NSInteger)pickerView:(UIPickerView *)thePickerView numberOfRowsInComponent:(NSInteger)component 
{             
    return [itemArray count];
}

- (NSString *)pickerView:(UIPickerView *)thePickerView titleForRow:(NSInteger)row forComponent:(NSInteger)component 
{
    return [itemArray objectAtIndex:row];
}

- (void)pickerView:(UIPickerView *)thePickerView didSelectRow:(NSInteger)row inComponent:(NSInteger)component 
{    
    [[self.superTable cellForRowAtIndexPath:selectedIndexPath].detailTextLabel setText:[itemArray objectAtIndex:row]];
    [self.superTable reloadData];
}

#pragma mark - Touches Call Backs

- (void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event {
    [[self nextResponder] touchesEnded:touches withEvent:event];
    [[self nextResponder] touchesCancelled:touches withEvent:event];
}

- (void)touchesMoved:(NSSet *)touches withEvent:(UIEvent *)event {
    [[self nextResponder] touchesEnded:touches withEvent:event];
    [[self nextResponder] touchesCancelled:touches withEvent:event];
}

- (void)touchesEnded:(NSSet *)touches withEvent:(UIEvent *)event {
}

- (void)touchesCancelled:(NSSet *)touches withEvent:(UIEvent *)event {
}


@end
