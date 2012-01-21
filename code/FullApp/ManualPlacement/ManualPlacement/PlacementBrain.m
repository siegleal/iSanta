//
//  PlacementBrain.m
//  ManualPlacement
//
//  Created by CSSE Department on 1/21/12.
//  Copyright (c) 2012 Rose-Hulman Institute of Technology. All rights reserved.
//

#import "PlacementBrain.h"

@implementation PlacementBrain
@synthesize targetImage = _targetImage;
@synthesize points = _points;

- (UIImage *)targetImage
{
    /*get image*/
    //
    //
    
    NSBundle *appBundle = [NSBundle mainBundle];
    NSString *path = [appBundle pathForResource:@"TakePhoto" 
                                         ofType:@"png"];
    //
    //
    //
    if (!_targetImage) _targetImage = [[UIImage alloc] initWithContentsOfFile:path];
    return _targetImage;
}

- (NSMutableSet *)points
{
    if (!_points) _points = [[NSMutableSet alloc] init];
    return _points;
}

- (void)addPoint:(Impact *)pointToAdd
{
    [self.points addObject:pointToAdd];
}

- (void)removePointatX:(int)x andY:(int)y
{
    NSEnumerator *e = [self.points objectEnumerator];
    Impact *i = e.nextObject;
    while(i)
    {
        if (i.position.x == x && i.position.y == y)
        {
            [self.points removeObject:i];
        }
    }
}


@end
