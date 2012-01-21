//
//  ManualPlacementViewController.m
//  ManualPlacement
//
//  Created by CSSE Department on 1/21/12.
//  Copyright (c) 2012 Rose-Hulman Institute of Technology. All rights reserved.
//

#import "ManualPlacementViewController.h"

@implementation ManualPlacementViewController

- (IBAction)actionButtonPress:(id)sender {
    UIActionSheet *actionSheet = [[UIActionSheet alloc] initWithTitle:@"Select option" delegate:self cancelButtonTitle:@"Cancel" destructiveButtonTitle:@"Remove point" otherButtonTitles:@"Add point",@"Move point", nil];
    [actionSheet showInView:self.view];                                  
                                  
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

@end
