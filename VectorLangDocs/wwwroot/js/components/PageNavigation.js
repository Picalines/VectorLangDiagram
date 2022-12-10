export function QueryNavSections() {
    const sections = [];

    for (const element of document.querySelectorAll('[id][data-nav-anchor],[id][data-nav-section]')) {
        const elementId = element.getAttribute('id');
        const anchor = element.getAttribute('data-nav-anchor');
        const section = element.getAttribute('data-nav-section');

        if (section) {
            sections.push({ headerTitle: section, headerAnchor: elementId, anchors: [] });
        }
        else if (anchor) {
            let lastSection = sections[sections.length - 1];

            if (!lastSection) {
                lastSection = { headerTitle: null, headerAnchor: null, anchors: [] };
                sections.push(lastSection);
            }

            lastSection.anchors.push({ title: anchor, anchor: elementId });
        }
    }

    return sections;
}
