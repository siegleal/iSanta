//
//  ManualPlacementViewController.m
//  ManualPlacement
//
//  Created by CSSE Department on 1/21/12.
//  Copyright (c) 2012 Rose-Hulman Institute of Technology. All rights reserved.
//

#import "ManualPlacementViewController.h"
@interface ManualPlacementViewController()
    @property (nonatomic,strong) UIActionSheet *actionSheet;

@end



@implementation ManualPlacementViewController
@synthesize brain = _brain;
@synthesize imageView = _imageView;
@synthesize actionSheet = _actionSheet;


int currentOp = 1;

- (void) viewDidLoad
{
    self.imageView.image = self.brain.targetImage;
}

- (PlacementBrain *)brain
{
    if (!_brain) _brain = [[PlacementBrain alloc] init];
    return _brain;
}

- (UIActionSheet *)actionSheet
{
    if (!_actionSheet) _actionSheet = [[UIActionSheet alloc] initWithTitle:@"Select option" delegate:self cancelButtonTitle:@"Cancel" destructiveButtonTitle:@"Remove point" otherButtonTitles:@"Add point",@"Move point", nil];
    return _actionSheet;
}

- (IBAction)actionButtonPress:(id)sender {
    [self.actionSheet showInView:self.view];                                  
                                  
}

- (void)actionSheet: (UIActionSheet *)actionSheet clickedButtonAtIndex:(NSInteger)buttonIndex
{
    NSLog(@"CurrentOp was %d",currentOp);
    currentOp = buttonIndex;
    NSLog(@"CurrentOp = %d",currentOp);
    if (buttonIndex == 0) //Remove points
    {
        NSLog(@"Current operation: Remove point");
    }
    else if (buttonIndex == 1) //Add point
    {
        NSLog(@"Current operation: Add point");
    }
    else if (buttonIndex == 2) //modify point
    {
        NSLog(@"Current operation: Modify point");
    }
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

- (void)viewDidUnload {
    [self setImageView:nil];
    [super viewDidUnload];
}
@end
